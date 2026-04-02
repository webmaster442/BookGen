//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Templates;

using Microsoft.Extensions.Logging;

using PreMailer.Net;

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

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        logger.LogInformation("Replacing non xhtml elements with xhtml compatible ones...");

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

        var renderer = new TemplateEngine(logger, environment);

        var viewData = new ViewData
        {
            Host = "",
            Content = State.Buffer.ToString(),
            LastModified = DateTime.UtcNow,
            Title = environment.Configuration.BookTitle,
            AdditionalData = new(),
        };

        var withCss = renderer.Render(tempate, viewData);
        logger.LogInformation("Moving css into inline atttibutes...");

        using var pm = new PreMailer.Net.PreMailer(withCss);
        var result = pm.MoveCssInline(removeStyleElements: false, preserveMediaQueries: true);


        if (result.Warnings.Count> 0)
        {
            foreach (string warning in result.Warnings)
            {
                logger.LogWarning("Xhtml: {Warning}", warning);
            }
        }


        logger.LogInformation("Writing xhtml file...");
        await environment.Output.WriteAllTextAsync("print.xhtml.html", result.Html);

        return StepResult.Success;
    }
}
