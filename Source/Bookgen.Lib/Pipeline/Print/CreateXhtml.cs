using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Print;
internal sealed class CreateXhtml : IPipeLineStep
{
    private readonly Dictionary<string, string> _tagreplacements;

    public CreateXhtml()
    {
        _tagreplacements = new Dictionary<string, string>()
        {
            { "abbr", "span" },
            { "acronym", "span" },
            { "address", "div" },
            { "article", "div" },
            { "aside", "div" },
            { "canvas", "div" },
            { "cite", "span" },
            { "dd", "span" },
            { "details", "div" },
            { "dfn", "span" },
            { "dl", "div" },
            { "dt", "span" },
            { "figcaption", "p" },
            { "figure", "div" },
            { "footer", "div" },
            { "header", "div" },
            { "kbd", "span" },
            { "nav", "div" },
            { "samp", "span" },
            { "section", "div" },
            { "var", "span" },
        };
    }

    private string ConvertHtml5TagsToXhtmlCompatible(string input)
    {
        var buffer = new StringBuilder(input);

        foreach (KeyValuePair<string, string> elementToReplace in _tagreplacements)
        {
            //starting bracket
            buffer.Replace($"<{elementToReplace.Key}", $"<{elementToReplace.Value}");
            //end
            buffer.Replace($"</{elementToReplace.Key}>", $"</{elementToReplace.Value}>");
        }

        return buffer.ToString();
    }

    public Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
