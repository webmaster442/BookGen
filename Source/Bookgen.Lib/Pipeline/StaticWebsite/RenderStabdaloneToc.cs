using Bookgen.Lib.Templates;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;


namespace Bookgen.Lib.Pipeline.StaticWebsite;
internal class RenderStabdaloneToc : IPipeLineStep<StaticWebState>
{
    public RenderStabdaloneToc(StaticWebState staticWebState)
    {
        State = staticWebState;
    }

    public StaticWebState State { get; }

    public async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {

        if (string.IsNullOrEmpty(environment.Configuration.StaticWebsiteConfig.TocConfiguration.StandaloneFileName))
        {
            logger.LogWarning("Standalone TOC file name is not set, skipping rendering.");
            return StepResult.Success;
        }

        var renderer = new TemplateEngine();

        string tempate = await environment.GetTemplate(frontMatterTemplate: string.Empty,
                                                       fallbackTemplate: BundledAssets.TemplateStaticWeb,
                                                       defaultTemplateSelector: cfg => cfg.StaticWebsiteConfig.DefaultTempate);

        var viewData = new StaticViewData
        {
            Content = State.Toc,
            Title = environment.Configuration.BookTitle,
            AdditionalData = new(),
            Toc = string.Empty,
        };

        string finalContent = renderer.Render(tempate, viewData);

        var outputName = environment.Source.GetFileNameInTargetFolder(environment.Output, environment.Configuration.StaticWebsiteConfig.TocConfiguration.StandaloneFileName);

        await environment.Output.WriteAllTextAsync(outputName, finalContent);

        logger.LogInformation("Standalone TOC rendered to {OutputName}", outputName);
        return StepResult.Success;
    }
}
