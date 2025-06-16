using System.Text;
using System.Text.RegularExpressions;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using static SkiaSharp.HarfBuzz.SKShaper;

namespace Bookgen.Lib.Templates;

public sealed partial class TemplateEngine
{
    private readonly StringComparer _comparer;
    private readonly Dictionary<string, Func<string[], string>> _lambdaTable;
    private readonly ILogger _logger;
    private readonly DefaultFunctions _functions;

    public void RegisterFunction(string name, Func<string[], string> function)
    {
        _lambdaTable.Add(name, function);
    }

    public TemplateEngine(ILogger logger, IAssetSource assetSource)
    {
        _comparer = StringComparer.InvariantCultureIgnoreCase;
        _logger = logger;
        _functions = new DefaultFunctions(assetSource);
        _lambdaTable = new Dictionary<string, Func<string[], string>>(_comparer)
        {
            { "BuildDate", _functions.BuildDate },
            { "JSPageToc", _functions.JSPageToc },
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
        var dataTable = viewData.GetDataTable(_comparer);

        StringBuilder lineBuffer = new(120);

        using StringReader reader = new(template.Replace("{{content}}", viewData.Content, StringComparison.InvariantCultureIgnoreCase));
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

                    if (!_lambdaTable.TryGetValue(functionName, out var function))
                    {
                        _logger.LogWarning("Function {FunctionName} is not registered.", functionName);
                        lineBuffer.Append($"Function {functionName} is not registered.");
                        continue;
                    }
                    string result = _lambdaTable[functionName].Invoke(arguments);
                    lineBuffer.Append(result);
                }
                else
                {
                    string key = templatePart.Value[2..^2];

                    if (key.Equals("content", StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new InvalidOperationException("Content found in markdown document. Recursive replacement detected.");
                    }

                    if (!dataTable.TryGetValue(key, out var value))
                    {
                        _logger.LogWarning("Key {Key} is not found in data table.", key);
                        lineBuffer.Append($"Key {key} is not found in data table.");
                        continue;
                    }

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
