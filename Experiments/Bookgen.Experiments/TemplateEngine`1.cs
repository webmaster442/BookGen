using System.Globalization;
using System.Reflection;
using System.Text;

namespace Bookgen.Experiments;

public sealed class TemplateEngine<TModel> : TemplateEngine
{
    private readonly PropertyInfo[] _properties;
    private readonly bool _emitNullString;

    public TemplateEngine(bool emitNullString)
    {
        _properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        _emitNullString = emitNullString;
    }


    private const string ModelVariableName = "_model";

    private Dictionary<string, object?> GetValues(TModel? model)
    {
        if (model is IDictionaryConvertible dictionaryConvertible)
            return dictionaryConvertible.ToDictionary();

        Dictionary<string, object?> values = new();

        values.Add(ModelVariableName, model);
        foreach (PropertyInfo property in _properties)
        {
            object? value = property.GetValue(model);
            values[property.Name] = value;
        }
        return values;
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

    private string GetStr(object? value)
    {
        if (value is IFormattable formattable)
        {
            return formattable.ToString(null, CultureInfo.InvariantCulture);
        }
        return value?.ToString() ?? (_emitNullString ? "null" : string.Empty);
    }

    private string Evaluate(string expression, Dictionary<string, object?> values)
    {
        Func<IReadOnlyDictionary<string, object?>, object?> compiled = GetCompiledExpression(expression);
        object? result = compiled(values);
        return GetStr(result);
    }
}
