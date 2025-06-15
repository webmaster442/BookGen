using System.Diagnostics;

using Bookgen.Lib.Domain;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Templates;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class RenderStaticPages : PipeLineStep<StaticWebState>
{
    public RenderStaticPages(StaticWebState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var imgService = new ImgService(environment.Source, environment.Configuration.StaticWebsiteConfig.Images);
        var cached = new CachedImageService(imgService);
        var renderer = new TemplateEngine(logger, environment);

        using var settings = new RenderSettings
        {
            CssClasses = environment.Configuration.StaticWebsiteConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = environment.Configuration.StaticWebsiteConfig.DeployHost,
            PrismJsInterop = null,
        };

        using var markdown = new MarkdownToHtml(cached, settings);

        ParallelOptions options = new ParallelOptions
        {
            CancellationToken = cancellationToken,
        };

#if DEBUG
        if (Debugger.IsAttached)
        {
            options.MaxDegreeOfParallelism = 1;
        }
#endif

        await Parallel.ForEachAsync(environment.TableOfContents.GetFiles(), options, async (file, token) =>
        {
            if (token.IsCancellationRequested) return;

            SourceFile sourceData = State.SourceFiles[file];

            string tempate = await environment.GetTemplate(frontMatterTemplate: sourceData.FrontMatter.Template,
                                                           fallbackTemplate: BundledAssets.TemplateStaticWeb,
                                                           defaultTemplateSelector: cfg => cfg.StaticWebsiteConfig.DefaultTempate);

            var viewData = new StaticViewData
            {
                Host = environment.Configuration.StaticWebsiteConfig.DeployHost,
                Content = markdown.RenderMarkdownToHtml(sourceData.Content),
                Title = sourceData.FrontMatter.Title,
                AdditionalData = sourceData.FrontMatter.Data ?? new(),
                LastModified = sourceData.LastModified,
                Toc = State.Toc,
            };

            string finalContent = renderer.Render(tempate, viewData);

            var outputName = environment.Source.GetFileNameInTargetFolder(environment.Output, file, ".html");

            await environment.Output.WriteAllTextAsync(outputName, finalContent);
        });

        return StepResult.Success;
    }
}