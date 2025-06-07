using Bookgen.Lib.Templates;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Print;

internal sealed class WriteXHtml : PipeLineStep<PrintState>
{
    private readonly Dictionary<string, string> _tagreplacements;

    public WriteXHtml(PrintState state) : base(state)
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

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Writing print xhtml...");

        foreach (KeyValuePair<string, string> elementToReplace in _tagreplacements)
        {
            //starting bracket
            State.Buffer.Replace($"<{elementToReplace.Key}", $"<{elementToReplace.Value}");
            //end
            State.Buffer.Replace($"</{elementToReplace.Key}>", $"</{elementToReplace.Value}>");
        }

        string tempate = await environment.GetTemplate(frontMatterTemplate: null,
                                                       fallbackTemplate: BundledAssets.TemplatePrint,
                                                       defaultTemplateSelector: cfg => cfg.PrintConfig.DefaultTempate);

        var renderer = new TemplateEngine();

        var viewData = new ViewData
        {
            Host = "",
            Content = State.Buffer.ToString(),
            Title = environment.Configuration.BookTitle,
            AdditionalData = new(),
        };

        using var writer = environment.Output.CreateTextWriter("print.xhtml.html");
        renderer.Render(writer, tempate, viewData);

        return StepResult.Success;
    }
}
