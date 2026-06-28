using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Bookgen.Experiments.Expressions;

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

    public void Render(string template, TextWriter target, TModel model)
    {
        Dictionary<string, object?> values = GetValues(model);
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
                target.Write(replacement);
                i = end + 2; // Move past the closing braces
            }
            else
            {
                target.Write(template[i]);
                i++;
            }
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
        Expression ex = ExpressionFactory.Create(expression, values, _functions);
        if (ex is ConstantExpression constant)
        {
            return GetStr(constant.Value);
        }
        else if (ex is InvocationExpression invocation)
        {
            Delegate lambda = Expression.Lambda(invocation).Compile();
            object? result = lambda.DynamicInvoke();
            return GetStr(result);
        }
        return ex.ToString();
    }
}
