//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.Epub;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Templates;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using static Bookgen.Lib.Pipeline.Epub.EpubState;

namespace Bookgen.Lib.Pipeline.Epub;

internal class CreateHtmlPages : PipeLineStep<EpubState>
{
    private readonly IMemoryCache _memoryCache;

    public CreateHtmlPages(EpubState state, IMemoryCache memoryCache) : base(state)
    {
        _memoryCache = memoryCache;
    }

    private string EpubImageRewrite(ImageResult result)
    {
        var name = IdGenerator.GenerateImageFileName(result.OriginalName, result.ImageType);
        State.ImagesData.TryAdd(name, result.Data);
        return name;
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        var imgService = new ImgService(environment.Source, logger, new ImageConfig
        {
            SvgRecode = SvgRecodeOption.AsWebp,
            ResizeAndRecodeImages = ImgRecodeOption.AsPng,
            ImageQualityOnResize = 90,
            ResizeWith = 1600,
            ResizeHeight = 1600,
        });
        var cached = new CachedImageService(imgService, _memoryCache);

        using var settings = new RenderSettings(cached)
        {
            CssClasses = environment.Configuration.PrintConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = string.Empty,
            PrismJsInterop = new SyntaxRenderJsInterop(environment),
            OffsetHeadingsBy = 0,
            AutoEmbedSupportedLinks = false,
            ImageUrlRewriter = EpubImageRewrite
        };

        using var markdown = new MarkdownConverter(settings);

        var renderer = new TemplateEngine(logger, environment);

        int chapterId = 1;
        int fileId = 1;

        string template = environment.GetAsset("Epub.html");

        logger.LogInformation("Rendering cover/index...");

        await RenderIndex(environment, logger, markdown, renderer, template);

        foreach (TocChapter chapter in environment.TableOfContents.Chapters)
        {
            logger.LogInformation("Rendering chapter {chapter}...", chapter.Title);
            fileId = 1;
            var chapterLinks = new List<ChapterItem>();
            foreach (var page in chapter.Files)
            {
                logger.LogDebug("Rendering {file}...", page);

                SourceFile sourceData = await environment.Source.GetSourceFile(page, logger);

                string targetfileName = $"content/{chapterId}_{fileId}.xhtml";

                var viewData = new ViewData
                {
                    Content = markdown.RenderMarkdownToHtml(sourceData.Content).MakeSelfClosingTagsXmlCompatible(),
                    Title = sourceData.FrontMatter.Title,
                    Host = string.Empty,
                    LastModified = sourceData.LastModified,
                };

                string html = renderer.Render(template, viewData);

                await State.EpubFile.AddAsync($"EPUB/{targetfileName}", html, Encoding.UTF8);

                var id = $"id-{IdGenerator.Generate32BitDeterministicId(targetfileName)}";

                State.PackageItems.Add(new PackageItem
                {
                    Href = targetfileName,
                    Id = id,
                    Mediatype = "application/xhtml+xml",
                });

                chapterLinks.Add(new ChapterItem(sourceData.FrontMatter.Title, targetfileName));

                ++fileId;
            }
            ++chapterId;
            State.TocData.Add(chapter.Title, chapterLinks);
        }
        return StepResult.Success;

    }

    private async Task RenderIndex(IBookEnvironment environment, ILogger logger, MarkdownConverter markdown, TemplateEngine renderer, string template)
    {
        SourceFile index = await environment.Source.GetSourceFile(environment.TableOfContents.IndexFile, logger);

        var indexView = new ViewData
        {
            Content = markdown.RenderMarkdownToHtml(index.Content).MakeSelfClosingTagsXmlCompatible(),
            Title = environment.Configuration.BookTitle,
            Host = string.Empty,
            LastModified = index.LastModified,
        };

        string indexHtml = renderer.Render(template, indexView);

        var name = "content/index.xhtml";

        await State.EpubFile.AddAsync($"EPUB/{name}", indexHtml, Encoding.UTF8);

        var id = $"id-{IdGenerator.Generate32BitDeterministicId(name)}";

        State.PackageItems.Add(new PackageItem
        {
            Href = name,
            Id = id,
            Mediatype = "application/xhtml+xml",
        });
        State.TocData.Add(environment.Configuration.BookTitle, [new ChapterItem(environment.Configuration.BookTitle, name)]);
    }
}
