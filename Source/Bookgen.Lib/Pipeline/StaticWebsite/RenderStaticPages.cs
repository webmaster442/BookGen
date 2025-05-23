using Bookgen.Lib.Domain;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Templates;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class RenderStaticPages : IPipeLineStep<StaticWebState>
{
    public RenderStaticPages(StaticWebState state)
    {
        State = state;
    }

    public StaticWebState State { get; }

    public async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var imgService = new ImgService(environment.Source, environment.Configuration.StaticWebsiteConfig.Images);
        var cached = new CachedImageService(imgService);
        var renderer = new TemplateEngine();

        using var settings = new RenderSettings
        {
            CssClasses = environment.Configuration.StaticWebsiteConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = environment.Configuration.StaticWebsiteConfig.DeployHost,
            PrismJsInterop = null,
        };

        using var markdown = new MarkdownToHtml(cached, settings);

        var files = environment.TableOfContents.Chapters.SelectMany(x => x.Files);

        await Parallel.ForEachAsync(files, cancellationToken, async (file, token) =>
        {
            if (token.IsCancellationRequested) return;

            SourceFile sourceData = environment.Source.GetSourceFile(file, logger);
            string tempate = await environment.GetTemplate(frontMatterTemplate: sourceData.FrontMatter.Template,
                                                           fallbackTemplate: BundledAssets.TemplateStaticWeb,
                                                           defaultTemplateSelector: cfg => cfg.StaticWebsiteConfig.DefaultTempate);

            var viewData = new ViewData
            {
                Content = markdown.RenderMarkdownToHtml(sourceData.Content),
                Title = sourceData.FrontMatter.Title,
                AdditionalData = sourceData.FrontMatter.Data ?? new(),
            };

            string finalContent = renderer.Render(tempate, viewData);

            var outputName = environment.Source.GetFileNameInTargetFolder(environment.Output, file);

            await environment.Output.WriteAllTextAsync(outputName, finalContent);
        });

        return StepResult.Success;
    }
}