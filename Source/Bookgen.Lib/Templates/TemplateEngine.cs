using System.Text;
using System.Text.RegularExpressions;

namespace Bookgen.Lib.Templates;

public sealed partial class TemplateEngine
{
    private readonly Dictionary<string, Func<string[], string>> _lambdaTable = new();

    private static string BuildDate(string[] arguments)
        => DateTime.Now.ToString("yy-MM-dd hh:mm:ss");

    public void RegisterFunction(string name, Func<string[], string> function)
    {
        _lambdaTable.Add(name, function);
    }

    public TemplateEngine()
    {
        _lambdaTable = new Dictionary<string, Func<string[], string>>
        {
            { "BuildDate", BuildDate }
        };
    }

    [GeneratedRegex(@"\b([\w\-]+)\s*\(\s*(?:""([^""]*)""(?:\s*,\s*""([^""]*)"")?(?:\s*,\s*""([^""]*)"")?(?:\s*,\s*""([^""]*)"")?(?:\s*,\s*""([^""]*)"")?(?:\s*,\s*""([^""]*)"")?(?:\s*,\s*""([^""]*)"")?(?:\s*,\s*""([^""]*)"")?)?\s*\)")]
    private static partial Regex FunctionRegex();

    [GeneratedRegex(@"\{\{([\w\-]+)\}\}")]
    private static partial Regex TemplatePartRegex();


    public string Render<TData>(string template, TData viewData) where TData : ViewData
    {
        var dataTavble = viewData.GetDataTable();

        StringBuilder rendered = new(template.Length + viewData.Content.Length + viewData.Title.Length);
        using StringReader reader = new(template);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var templatePartsInLine = TemplatePartRegex().Matches(line);

            if (templatePartsInLine.Count < 1)
            {
                rendered.AppendLine(line);
                continue;
            }

            foreach (Match templatePart in templatePartsInLine)
            {
                if (FunctionRegex().IsMatch(templatePart.Value))
                {
                    string[] functionCall = FunctionRegex().Split(templatePart.Value);
                }
                else
                {
                    
                }
            }
        }

        return rendered.ToString();
    }
}
