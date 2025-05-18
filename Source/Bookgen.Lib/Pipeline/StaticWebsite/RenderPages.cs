using Bookgen.Lib.ImageService;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Templates;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class RenderPages : IPipeLineStep
{
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
            var sourceData = environment.Source.GetSourceFile(file, logger);
            var tempate = await GetTemplate(sourceData.FrontMatter.Template, environment);

            var viewData = new ViewData
            {
                Content = markdown.RenderMarkdownToHtml(sourceData.Content),
                Title = sourceData.FrontMatter.Title,
            };

            string finalContent = renderer.Render(tempate, viewData);

            //get output file
            //write output file
        });

        return StepResult.Success;
    }

    private static async Task<string> GetTemplate(string? frontMatterTemplate, IBookEnvironment environment)
    {
        if (!string.IsNullOrEmpty(frontMatterTemplate))
        {
            return await environment.Source.ReadAllTextAsync(frontMatterTemplate);
        }

        if (!string.IsNullOrEmpty(environment.Configuration.StaticWebsiteConfig.DefaultTempate))
        {
            return await environment.Source.ReadAllTextAsync(environment.Configuration.StaticWebsiteConfig.DefaultTempate);
        }

        return environment.GetAsset(BundledAssets.TemplateStaticWeb);
    }
}