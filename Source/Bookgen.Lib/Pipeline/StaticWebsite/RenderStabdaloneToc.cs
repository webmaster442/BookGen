//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Templates;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;


namespace Bookgen.Lib.Pipeline.StaticWebsite;
internal class RenderStabdaloneToc : PipeLineStep<StaticWebState>
{
    public RenderStabdaloneToc(StaticWebState staticWebState) : base(staticWebState)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {

        if (string.IsNullOrEmpty(environment.Configuration.StaticWebsiteConfig.TocConfiguration.StandaloneFileName))
        {
            logger.LogWarning("Standalone TOC file name is not set, skipping rendering.");
            return StepResult.Success;
        }

        var renderer = new TemplateEngine(logger, environment);

        string tempate = await environment.GetTemplate(frontMatterTemplate: string.Empty,
                                                       fallbackTemplate: BundledAssets.TemplateStaticWeb,
                                                       defaultTemplateSelector: cfg => cfg.StaticWebsiteConfig.DefaultTempate);

        var viewData = new StaticViewData
        {
            Host = environment.Configuration.StaticWebsiteConfig.DeployHost,
            Content = State.Toc,
            Title = environment.Configuration.BookTitle,
            AdditionalData = new(),
            Toc = string.Empty,
            LastModified = DateTime.UtcNow
        };

        string finalContent = renderer.Render(tempate, viewData);

        var outputName = environment.Source.GetFileNameInTargetFolder(environment.Output, environment.Configuration.StaticWebsiteConfig.TocConfiguration.StandaloneFileName, ".html");

        await environment.Output.WriteAllTextAsync(outputName, finalContent);

        logger.LogInformation("Standalone TOC rendered to {OutputName}", outputName);
        return StepResult.Success;
    }
}
