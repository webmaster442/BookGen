using System.Data;
using System.Text;

namespace Bookgen.Experiments;

public sealed class TemplateEngine
{
    public string Render<T>(string template, T model)
    {
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
                string replacement = Evaluate(expression);
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

    private string Evaluate(string expression)
    {
        throw new NotImplementedException();
    }
}
