namespace Bookgen.Experiments;

public abstract class TemplateEngine
{
    protected readonly Dictionary<string, List<Delegate>> _functions;

    protected TemplateEngine()
    {
        _functions = new Dictionary<string, List<Delegate>>();
    }

    private void Register(string name, Delegate function)
    {
        if (!_functions.TryGetValue(name, out List<Delegate>? value))
        {
            value = new List<Delegate>();
            _functions[name] = value;
        }

        value.Add(function);
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
