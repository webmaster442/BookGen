using System.Collections.Concurrent;

using Bookgen.Experiments.Expressions;

namespace Bookgen.Experiments;

public abstract class TemplateEngine
{
    private readonly Dictionary<string, List<FunctionOverload>> _functions;

    private readonly ConcurrentDictionary<string, Func<IReadOnlyDictionary<string, object?>, object?>> _expressionCache;
    private readonly Func<string, Func<IReadOnlyDictionary<string, object?>, object?>> _compileExpression;

    protected TemplateEngine()
    {
        _functions = new Dictionary<string, List<FunctionOverload>>();
        _expressionCache = new ConcurrentDictionary<string, Func<IReadOnlyDictionary<string, object?>, object?>>(StringComparer.Ordinal);
        _compileExpression = expression => ExpressionFactory.Compile(expression, _functions);
    }

    /// <summary>
    /// Returns a delegate for the given expression, compiling and caching it on first use.
    /// The compiled delegate is independent of the model values, so it can be reused across renders.
    /// </summary>
    protected Func<IReadOnlyDictionary<string, object?>, object?> GetCompiledExpression(string expression)
        => _expressionCache.GetOrAdd(expression, _compileExpression);

    private void Register(string name, Delegate function)
    {
        if (!_functions.TryGetValue(name, out List<FunctionOverload>? value))
        {
            value = new List<FunctionOverload>();
            _functions[name] = value;
        }

        value.Add(new FunctionOverload(function));

        // A newly registered function can change how previously seen expressions resolve.
        _expressionCache.Clear();
    }

    public void RegisterFunction(string name, Func<string> function)
        => Register(name, function);

    public void RegisterFunction(string name, Func<object, string> function)
        => Register(name, function);

    public void RegisterFunction(string name, Func<object, object, string> function)
        => Register(name, function);

    public void RegisterFunction(string name, Func<object, object, object, string> function)
        => Register(name, function);

    public void RegisterFunction(string name, Func<object[], string> function)
        => Register(name, function);
}
