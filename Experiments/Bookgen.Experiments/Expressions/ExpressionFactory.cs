using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Bookgen.Experiments.Expressions;

internal ref struct ExpressionFactory
{
    // Variables are read from this single parameter at invoke time instead of being baked
    // in as constants, so a compiled expression no longer depends on a specific model and
    // can be cached and reused across renders. The instance is immutable and shared by all
    // compiled lambdas.
    private static readonly ParameterExpression VariablesParameter =
        Expression.Parameter(typeof(IReadOnlyDictionary<string, object?>), "variables");

    private static readonly MethodInfo GetVariableMethod =
        typeof(ExpressionFactory).GetMethod(nameof(GetVariable), BindingFlags.NonPublic | BindingFlags.Static)!;

    public static Func<IReadOnlyDictionary<string, object?>, object?> Compile(
        string expressionString,
        Dictionary<string, List<FunctionOverload>> functions)
    {
        TokenCollection tokens = Tokenizer.Tokenize(expressionString, x => functions.ContainsKey(x));

        // The collection is positioned before the first token, advance onto it.
        tokens.Next();

        // A lone variable reference is the most common template token; serve it with a direct
        // dictionary lookup instead of emitting and JIT-compiling IL.
        if (tokens.Count == 2 && tokens.CurrentToken.Type == TokenType.Variable)
        {
            string name = tokens.CurrentToken.Value;
            return variables => GetVariable(variables, name);
        }

        Expression body = ParseExpression(tokens, functions);

        if (tokens.CurrentToken.Type != TokenType.EOF)
            throw new InvalidOperationException($"Unexpected token after expression: {tokens.CurrentToken}");

        // Constant (literal / constant-folded) expressions don't need IL emission either.
        if (body is ConstantExpression constant)
        {
            object? value = constant.Value;
            return _ => value;
        }

        if (body.Type != typeof(object))
            body = Expression.Convert(body, typeof(object));

        return Expression
            .Lambda<Func<IReadOnlyDictionary<string, object?>, object?>>(body, VariablesParameter)
            .Compile();
    }

    private static Expression ParseExpression(TokenCollection tokens,
                                              Dictionary<string, List<FunctionOverload>> functions)
    {
        Token token = tokens.CurrentToken;
        switch (token.Type)
        {
            case TokenType.Integer:
                tokens.Eat(TokenType.Integer);
                return Expression.Constant(long.Parse(token.Value, CultureInfo.InvariantCulture));
            case TokenType.Double:
                tokens.Eat(TokenType.Double);
                return Expression.Constant(double.Parse(token.Value, CultureInfo.InvariantCulture));
            case TokenType.Boolean:
                tokens.Eat(TokenType.Boolean);
                return Expression.Constant(bool.Parse(token.Value));
            case TokenType.String:
                tokens.Eat(TokenType.String);
                return Expression.Constant(token.Value);
            case TokenType.Variable:
                tokens.Eat(TokenType.Variable);
                return Expression.Call(GetVariableMethod, VariablesParameter, Expression.Constant(token.Value));
            case TokenType.Function:
                return ParseFunction(tokens, functions);
            case TokenType.OpenParen:
                tokens.Eat(TokenType.OpenParen);
                Expression inner = ParseExpression(tokens, functions);
                tokens.Eat(TokenType.CloseParen);
                return inner;
            default:
                throw new InvalidOperationException($"Unexpected token: {token}");
        }
    }

    private static InvocationExpression ParseFunction(TokenCollection tokens,
                                                      Dictionary<string, List<FunctionOverload>> functions)
    {
        Token token = tokens.CurrentToken;
        List<FunctionOverload> overloads = functions[token.Value];

        tokens.Eat(TokenType.Function);
        tokens.Eat(TokenType.OpenParen);

        var arguments = new List<Expression>();
        if (tokens.CurrentToken.Type != TokenType.CloseParen)
        {
            arguments.Add(ParseExpression(tokens, functions));
            while (tokens.CurrentToken.Type == TokenType.ArgumentDelimiter)
            {
                tokens.Eat(TokenType.ArgumentDelimiter);
                arguments.Add(ParseExpression(tokens, functions));
            }
        }

        tokens.Eat(TokenType.CloseParen);

        return BuildInvocation(overloads, arguments);
    }

    private static object? GetVariable(IReadOnlyDictionary<string, object?> variables, string name)
    {
        if (!variables.TryGetValue(name, out object? value))
            throw new InvalidOperationException($"Unknown variable: {name}");

        return value;
    }

    private static InvocationExpression BuildInvocation(List<FunctionOverload> overloads, List<Expression> arguments)
    {
        foreach (FunctionOverload overload in overloads)
        {
            if (!overload.IsParamsArray && overload.ParameterTypes.Length == arguments.Count)
                return BuildRegularFunction(overload, arguments);
        }

        foreach (FunctionOverload overload in overloads)
        {
            if (overload.IsParamsArray)
                return BuildArgsFunction(overload, arguments);
        }

        throw new InvalidOperationException($"No suitable overload found that takes {arguments.Count} argument(s)");
    }

    private static InvocationExpression BuildArgsFunction(FunctionOverload overload, List<Expression> arguments)
    {
        var parameters = new Expression[arguments.Count];
        for (int i = 0; i < arguments.Count; i++)
        {
            parameters[i] = Expression.Convert(arguments[i], typeof(object));
        }

        return Expression.Invoke(Expression.Constant(overload.Function), Expression.NewArrayInit(typeof(object), parameters));
    }

    private static InvocationExpression BuildRegularFunction(FunctionOverload overload, List<Expression> arguments)
    {
        Type[] parameterTypes = overload.ParameterTypes;

        for (int i = 0; i < arguments.Count; i++)
        {
            Type parameterType = parameterTypes[i];
            if (arguments[i].Type != parameterType)
                arguments[i] = Expression.Convert(arguments[i], parameterType);
        }

        return Expression.Invoke(Expression.Constant(overload.Function), arguments);
    }
}
