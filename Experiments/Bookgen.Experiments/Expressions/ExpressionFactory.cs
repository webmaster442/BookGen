using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Bookgen.Experiments.Expressions;

internal ref struct ExpressionFactory
{
    public static Expression Create(string expressionString,
                                    Dictionary<string, object?> variables,
                                    Dictionary<string, Delegate> functions)
    {
        TokenCollection tokens = Tokenizer.Tokenize(expressionString, x => functions.ContainsKey(x));

        // The collection is positioned before the first token, advance onto it.
        tokens.Next();

        Expression result = ParseExpression(tokens, variables, functions);

        if (tokens.CurrentToken.Type != TokenType.EOF)
            throw new InvalidOperationException($"Unexpected token after expression: {tokens.CurrentToken}");

        return result;
    }

    private static Expression ParseExpression(TokenCollection tokens,
                                              Dictionary<string, object?> variables,
                                              Dictionary<string, Delegate> functions)
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
                return CreateVariable(token.Value, variables);
            case TokenType.Function:
                return ParseFunction(tokens, variables, functions);
            case TokenType.OpenParen:
                tokens.Eat(TokenType.OpenParen);
                Expression inner = ParseExpression(tokens, variables, functions);
                tokens.Eat(TokenType.CloseParen);
                return inner;
            default:
                throw new InvalidOperationException($"Unexpected token: {token}");
        }
    }

    private static InvocationExpression ParseFunction(TokenCollection tokens,
                                                      Dictionary<string, object?> variables,
                                                      Dictionary<string, Delegate> functions)
    {
        Token token = tokens.CurrentToken;
        Delegate function = functions[token.Value];

        tokens.Eat(TokenType.Function);
        tokens.Eat(TokenType.OpenParen);

        var arguments = new List<Expression>();
        if (tokens.CurrentToken.Type != TokenType.CloseParen)
        {
            arguments.Add(ParseExpression(tokens, variables, functions));
            while (tokens.CurrentToken.Type == TokenType.ArgumentDelimiter)
            {
                tokens.Eat(TokenType.ArgumentDelimiter);
                arguments.Add(ParseExpression(tokens, variables, functions));
            }
        }

        tokens.Eat(TokenType.CloseParen);

        return BuildInvocation(function, arguments);
    }

    private static ConstantExpression CreateVariable(string name, Dictionary<string, object?> variables)
    {
        if (!variables.TryGetValue(name, out object? value))
            throw new InvalidOperationException($"Unknown variable: {name}");

        return Expression.Constant(value, value?.GetType() ?? typeof(object));
    }

    private static InvocationExpression BuildInvocation(Delegate function, List<Expression> arguments)
    {
        MethodInfo invoke = function.GetType().GetMethod("Invoke")
            ?? throw new InvalidOperationException($"The delegate '{function}' does not expose an Invoke method.");

        ParameterInfo[] parameters = invoke.GetParameters();
        if (parameters.Length != arguments.Count)
        {
            throw new InvalidOperationException(
                $"Function expects {parameters.Length} argument(s), but {arguments.Count} were supplied.");
        }

        for (int i = 0; i < arguments.Count; i++)
        {
            Type parameterType = parameters[i].ParameterType;
            if (arguments[i].Type != parameterType)
                arguments[i] = Expression.Convert(arguments[i], parameterType);
        }

        return Expression.Invoke(Expression.Constant(function), arguments);
    }
}
