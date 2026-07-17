//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Api.V1;
using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Lib.Pipeline;
using BookGen.Lib.Rendering.Images;
using BookGen.Lib.Rendering.Markdown;
using BookGen.Lib.Rendering.Markdown.RenderInterop;
using BookGen.Lib.Rendering.Templates;

using Microsoft.Extensions.Caching.Memory;

namespace BookGen.Infrastructure.Plugins.V1;

internal sealed class Renderer : IRenderer
{
    private readonly MarkdownRenderSettings _markdownRenderSettings;
    private readonly MarkdownConverter _markdownConverter;
    private readonly TemplateEngine _engine;
    private bool _disposed;

    public Renderer(RendererOptions options,
                    IBookEnvironment environment,
                    IMemoryCache memoryCache,
                    Microsoft.Extensions.Logging.ILogger logger)
    {
        var imgConfig = new ImageConfig
        {
            SvgRecode = MapSvg(options.SvgRecode),
            ResizeAndRecodeImages = MapResize(options.ImageRecode),
            ImageQualityOnResize = 90,
            ResizeWith = options.ResizeWidth,
            ResizeHeight = options.ResizeHeight,
        };

        var imgService = new ImgService(environment.Source, logger, imgConfig);
        var cachedImageService = new CachedImageService(imgService, memoryCache);

        _markdownRenderSettings = new MarkdownRenderSettings(cachedImageService)
        {
            CssClasses = Map(options.CssClasses),
            DeleteFirstH1 = options.DeleteFirstH1,
            HostUrl = options.HostUrl,
            RenderInterop = new RenderInterop(environment, environment.ProgramPathResolver, imgConfig),
            OffsetHeadingsBy = 0,
            AutoEmbedSupportedLinks = options.AutoEmbedSupportedLinks,
        };

        _markdownConverter = new MarkdownConverter(_markdownRenderSettings);
        _engine = new TemplateEngine(logger, environment);
    }

    public void Dispose()
    {
        _markdownConverter.Dispose();
        _markdownRenderSettings.Dispose();
        _disposed = true;
    }

    private static SvgRecodeOption MapSvg(RendererOptions.ImageOption svgRecode)
    {
        return svgRecode switch
        {
            RendererOptions.ImageOption.Passtrough => SvgRecodeOption.Passtrough,
            RendererOptions.ImageOption.AsPng => SvgRecodeOption.AsPng,
            RendererOptions.ImageOption.AsWebp => SvgRecodeOption.AsWebp,
            _ => throw new UnreachableException(),
        };
    }

    private static ImgRecodeOption MapResize(RendererOptions.ImageOption imageRecode)
    {
        return imageRecode switch
        {
            RendererOptions.ImageOption.Passtrough => ImgRecodeOption.Passtrough,
            RendererOptions.ImageOption.AsPng => ImgRecodeOption.AsPng,
            RendererOptions.ImageOption.AsWebp => ImgRecodeOption.AsWebp,
            _ => throw new UnreachableException(),
        };
    }

    private Lib.Domain.IO.Configuration.CssClasses Map(Api.V1.CssClasses cssClasses)
    {
        return new Lib.Domain.IO.Configuration.CssClasses
        {
            H1 = cssClasses.H1,
            H2 = cssClasses.H2,
            H3 = cssClasses.H3,
            Img = cssClasses.Img,
            Table = cssClasses.Table,
            Blockquote = cssClasses.Blockquote,
            Figure = cssClasses.Figure,
            FigureCaption = cssClasses.FigureCaption,
            Link = cssClasses.Link,
            Ol = cssClasses.Ol,
            Ul = cssClasses.Ul,
            Li = cssClasses.Li
        };
    }


    public string RenderMarkdownToRawHtml(string markdown)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _markdownConverter.RenderMarkdownToHtml(markdown);
    }

    public async Task<string> RenderMarkdownToHtml(string pageTemplate, IDocument document)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        (string content, IDocumentFrontMatter frontMatter) = await document.ReadContent();

        return RenderMarkdownToHtml(pageTemplate, (content, frontMatter));
    }

    public string RenderMarkdownToHtml(string pageTemplate, (string content, IDocumentFrontMatter frontMatter) docData)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var viewData = new ViewData
        {
            Title = docData.frontMatter.Title,
            Content = RenderMarkdownToRawHtml(docData.content),
            Host = _markdownRenderSettings.HostUrl ?? string.Empty,
            AdditionalData = docData.frontMatter.AdditionalData.ToDictionary(),
            LastModified = DateTime.Now,
        };

        return _engine.Render(pageTemplate, viewData);
    }

    public string RenderMarkdownToHtml(string pageTemplate, RenderTags tags)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var viewData = new ViewData
        {
            Title = tags.Title,
            Content = RenderMarkdownToRawHtml(tags.Content),
            Host = tags.Host,
            AdditionalData = tags.AdditionalData,
            LastModified = tags.LastModified,
        };

        return _engine.Render(pageTemplate, viewData);
    }
}
