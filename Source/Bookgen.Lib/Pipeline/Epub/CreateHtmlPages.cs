using System.Text;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.Epub;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Templates;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal class CreateHtmlPages : PipeLineStep<EpubState>
{
    public CreateHtmlPages(EpubState state) : base(state)
    {
    }

    public static string GenerateImageFileName(string orignalPath, ImageType imageType)
    {
        const uint prime = 0x01000193;
        uint hash = 0x811c9dc5;
        foreach (var chr in orignalPath)
        {
            hash = (hash ^ chr) * prime;
        }

        var extension = imageType switch
        {
            ImageType.Jpeg => "jpg",
            ImageType.Png => "png",
            ImageType.Gif => "gif",
            ImageType.Webp => "webp",
            ImageType.Svg => "svg",
            _ => throw new ArgumentOutOfRangeException(nameof(imageType), imageType, null)
        };

        return $"{Convert.ToHexString(BitConverter.GetBytes(hash))}.{extension}";
    }

    private string EpubImageRewrite(ImageResult result)
    {
        var name = GenerateImageFileName(result.OriginalName, result.ImageType);
        State.ImagesData.TryAdd(name, result.Data);
        return name;
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var imgService = new ImgService(environment.Source, new ImageConfig
        {
            SvgRecode = SvgRecodeOption.AsWebp,
            ResizeAndRecodeImagesToWebp = true,
            WebpQuality = 90,
            ResizeWith = 1600,
            ResizeHeight = 1600,
        });
        var cached = new CachedImageService(imgService);

        using var settings = new RenderSettings(cached)
        {
            CssClasses = environment.Configuration.PrintConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = string.Empty,
            PrismJsInterop = new PrismJsInterop(environment),
            OffsetHeadingsBy = 1,
            AutoEmbedSupportedLinks = false,
            ImageUrlRewriter = EpubImageRewrite
        };

        using var markdown = new MarkdownToHtml(settings);

        var renderer = new TemplateEngine(logger, environment);

        int chapterId = 1;
        int fileId = 1;

        string template = environment.GetAsset("Epub.html");

        foreach (var chapter in environment.TableOfContents.Chapters)
        {
            logger.LogInformation("Rendering chapter {chapter}...", chapter.Title);
            fileId = 1;
            foreach (var page in chapter.Files)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    logger.LogWarning("Cancellation requested. Stoping...");
                    return StepResult.Failure;
                }

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

                State.EpubFile.Add($"EPUB/{targetfileName}", html, Encoding.UTF8);

                State.PackageItems.Add(new PackageItem
                {
                    Href = targetfileName,
                    Id = targetfileName,
                    Mediatype = "application/xhtml+xml",
                });

                State.Spine.Itemref.Add(new PackageSpineItemref
                {
                    Idref = targetfileName,
                    Linear = State.Spine.Itemref.Count == 0 ? "yes" : null,
                });

                ++fileId;
            }
            ++chapterId;
        }
        return StepResult.Success;

    }
}
