//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Bookgen.Lib.Domain;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Templates;

using BookGen.Vfs;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class RenderIndexPage : PipeLineStep<StaticWebState>
{
    private readonly IMemoryCache _memoryCache;

    public RenderIndexPage(StaticWebState state, IMemoryCache memoryCache) : base(state)
    {
        _memoryCache = memoryCache;
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        var imgService = new ImgService(environment.Source, logger, environment.Configuration.StaticWebsiteConfig.Images);
        var cached = new CachedImageService(imgService, _memoryCache);
        var renderer = new TemplateEngine(logger, environment);

        using var settings = new MarkdownRenderSettings(cached)
        {
            CssClasses = environment.Configuration.StaticWebsiteConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = environment.Configuration.StaticWebsiteConfig.DeployHost,
            PrismJsInterop = null,
            AutoEmbedSupportedLinks = true,
            ImageRenderJsInterop = new ImageRenderJsInterop(environment, environment.Configuration.StaticWebsiteConfig.Images)
        };

        using var markdown = new MarkdownConverter(settings);

        SourceFile sourceData = State.SourceFiles[environment.TableOfContents.IndexFile];

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

        var outputName = environment.Source.GetFileNameInTargetFolder(environment.Output, "index.html", ".html");

        await environment.Output.WriteAllTextAsync(outputName, finalContent);


        return StepResult.Success;
    }
}
