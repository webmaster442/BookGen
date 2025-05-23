using System.Text;
using System.Text.RegularExpressions;

using Markdig.Extensions.TaskLists;

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

    [GeneratedRegex(@"\{\{([\w-\(\)\""\, -_])+\}\}")]
    private static partial Regex TemplatePartRegex();

    public string Render<TData>(string template, TData viewData) where TData : ViewData
    {
        using var stringWriter = new StringWriter(new StringBuilder(template.Length + viewData.Content.Length + viewData.Title.Length));
        Render(stringWriter, template, viewData);
        return stringWriter.ToString();
    }

    public void Render<TData>(TextWriter target, string template, TData viewData) where TData : ViewData
    {
        var dataTable = viewData.GetDataTable();

        StringBuilder lineBuffer = new(120);

        using StringReader reader = new(template);
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            var templatePartsInLine = TemplatePartRegex().Matches(line);

            if (templatePartsInLine.Count < 1)
            {
                target.WriteLine(line);
                continue;
            }

            int lastIndex = 0;
            lineBuffer.Clear();

            foreach (Match templatePart in templatePartsInLine)
            {
                lineBuffer.Append(line, lastIndex, templatePart.Index - lastIndex);
                if (FunctionRegex().IsMatch(templatePart.Value))
                {
                    string[] templateFunction = FunctionRegex().Split(templatePart.Value);
                    string functionName = templateFunction.Skip(1).First();
                    string[] arguments = templateFunction.Skip(2).TakeWhile(f => f != "}}").ToArray();
                    string result = _lambdaTable[functionName].Invoke(arguments);
                    lineBuffer.Append(result);
                }
                else
                {
                    string key = templatePart.Value[2..^2];
                    lineBuffer.Append(dataTable[key]);
                    
                }
                lastIndex = templatePart.Index + templatePart.Length;
            }
            lineBuffer.Append(line, lastIndex, line.Length - lastIndex);

            if (lineBuffer.Length > 0)
            {
                target.Write(lineBuffer);
                target.WriteLine();
            }
        }
    }
}
