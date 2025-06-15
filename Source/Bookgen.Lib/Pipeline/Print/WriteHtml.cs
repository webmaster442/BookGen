using Bookgen.Lib.Templates;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Print;

internal sealed class WriteHtml : PipeLineStep<PrintState>
{
    public WriteHtml(PrintState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Writing print html...");

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

        using var writer = environment.Output.CreateTextWriter("print.html");
        renderer.Render(writer, tempate, viewData);

        return StepResult.Success;
    }
}
