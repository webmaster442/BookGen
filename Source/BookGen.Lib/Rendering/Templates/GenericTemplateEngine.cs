using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text;

using Bookgen.Lib.Rendering.Templates.Expressions;

using Microsoft.Extensions.Logging;

namespace BookGen.Lib.Rendering.Templates;

public class GenericTemplateEngine<TModel>
{
    private readonly Dictionary<string, List<FunctionOverload>> _functions;
    private readonly ConcurrentDictionary<string, Func<IReadOnlyDictionary<string, object?>, object?>> _expressionCache;
    private readonly Func<string, Func<IReadOnlyDictionary<string, object?>, object?>> _compileExpression;
    private readonly PropertyInfo[] _properties;
    private readonly ILogger _logger;
    private readonly TemplateEngineOptions _options;

    private const string ModelVariableName = "_model";

    public GenericTemplateEngine(ILogger logger, TemplateEngineOptions options)
    {
        _functions = new Dictionary<string, List<FunctionOverload>>(options.PropertyNameComparer);
        _expressionCache = new ConcurrentDictionary<string, Func<IReadOnlyDictionary<string, object?>, object?>>(options.PropertyNameComparer);
        _compileExpression = expression => ExpressionFactory.Compile(expression, _functions);
        _properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        _logger = logger;
        _options = options;
    }

    private Func<IReadOnlyDictionary<string, object?>, object?> GetCompiledExpression(string expression)
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

    private Dictionary<string, object?> GetValues(TModel? model)
    {
        if (model is IDictionaryConvertible dictionaryConvertible)
            return dictionaryConvertible.ToDictionary(_options.PropertyNameComparer);

        Dictionary<string, object?> values = new();

        values.Add(ModelVariableName, model);
        foreach (PropertyInfo property in _properties)
        {
            object? value = property.GetValue(model);
            values[property.Name] = value;
        }
        return values;
    }


    private string GetStr(object? value)
    {
        if (value is IFormattable formattable)
        {
            return formattable.ToString(null, CultureInfo.InvariantCulture);
        }
        return value?.ToString() ?? (_options.EmitNullString ? "null" : string.Empty);
    }

    private string Evaluate(string expression, Dictionary<string, object?> values)
    {
        try
        {
            Func<IReadOnlyDictionary<string, object?>, object?> compiled = GetCompiledExpression(expression);
            object? result = compiled(values);
            return GetStr(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error evaluating expression: {Expression}", expression);
            return $"<p style=\"color:red\">{ex.Message}</p>";
        }
    }

    private static StringBuilder AllocateBuffer(string template, TModel? model)
    {
        int size = template.Length * 2;
        if (model is ICanProvideRoughSize roughSize)
        {
            size = template.Length + roughSize.CalculateRoughSize();
        }
        return new StringBuilder(size);
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

    public string Render(string template, TModel model)
    {
        StringBuilder buffer = GenericTemplateEngine<TModel>.AllocateBuffer(template, model);
        using var stringWriter = new StringWriter(buffer);
        Render(stringWriter, template, model);
        return buffer.ToString();
    }

    public void Render(TextWriter target, string template, TModel model)
    {
        Dictionary<string, object?> values = GetValues(model);
        int i = 0;
        while (i < template.Length)
        {
            int start = template.IndexOf("{{", i, StringComparison.Ordinal);
            if (start == -1)
            {
                target.Write(template[i..]);
                break;
            }

            if (start > i)
            {
                target.Write(template[i..start]);
            }

            int end = template.IndexOf("}}", start + 2, StringComparison.Ordinal);
            if (end == -1)
            {
                throw new InvalidOperationException("Unmatched opening braces in template.");
            }

            string expression = template[(start + 2)..end].Trim();
            target.Write(Evaluate(expression, values));

            i = end + 2;
        }
    }

}
