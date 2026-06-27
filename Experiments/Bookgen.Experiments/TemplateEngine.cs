using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Bookgen.Experiments.Expressions;

namespace Bookgen.Experiments;

public sealed class TemplateEngine<TModel>
{
    private readonly PropertyInfo[] _properties;
    private readonly Dictionary<string, Delegate> _functions;

    public TemplateEngine()
    {
        _properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        _functions = new();
    }

    public void RegisterFunction(string name, Delegate function)
    {
        _functions[name] = function;
    }

    private Dictionary<string, object> GetValues(TModel? model)
    {
        Dictionary<string, object> values = new();
        foreach (var property in _properties)
        {
            object? value = property.GetValue(model);
            values[property.Name] = value ?? string.Empty;
        }
        return values;
    }

    public string Render(string template, TModel model)
    {
        Dictionary<string, object> values = GetValues(model);
        var buffer = new StringBuilder(4096);
        int i = 0;
        string? expression = null;
        while (i < template.Length)
        {
            char current = template[i];
            char next = (i + 1 < template.Length) ? template[i + 1] : '\0';
            if (current == '{' && next == '{')
            {
                //store parts in buffer until we find the closing braces
                int start = i + 2;
                int end = template.IndexOf("}}", start);
                if (end == -1)
                {
                    throw new InvalidOperationException("Unmatched opening braces in template.");
                }
                expression = template[start..end].Trim();
                string replacement = Evaluate(expression, values);
                buffer.Append(replacement);
                i = end + 2; // Move past the closing braces
            }
            else
            {
                buffer.Append(template[i]);
                i++;
            }
        }
        return buffer.ToString();
    }

    private string Evaluate(string expression, Dictionary<string, object> values)
    {
        Expression ex = ExpressionFactory.Create(expression, values, _functions);
        return ex.ToString();
    }
}
