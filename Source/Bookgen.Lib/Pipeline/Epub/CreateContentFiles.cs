using Bookgen.Lib.Domain;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;
internal class CreateContentFiles : PipeLineStep<EpubState>
{
    private readonly Dictionary<string, string> _imageContents;

    public CreateContentFiles(EpubState state) : base(state)
    {
        _imageContents = new Dictionary<string, string>();
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
            ImageType.Jpeg => ".jpg",
            ImageType.Png => ".png",
            ImageType.Gif => ".gif",
            ImageType.Webp => ".webp",
            ImageType.Svg => ".svg",
            _ => throw new ArgumentOutOfRangeException(nameof(imageType), imageType, null)
        };

        return $"{Convert.ToHexString(BitConverter.GetBytes(hash))}.{extension}";
    }

    private string EpubImageRewrite(ImageResult result)
    {
        var name = GenerateImageFileName(result.OriginalName, result.ImageType);
        _imageContents.TryAdd(name, result.Data);
        return name;
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var imgService = new ImgService(environment.Source, environment.Configuration.StaticWebsiteConfig.Images);
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

        int chapterId = 1;
        int fileId = 1;

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

                var html = markdown.RenderMarkdownToHtml(sourceData.Content).MakeSelfClosingTagsXmlCompatible();

                ++fileId;
            }
            ++chapterId;
        }
        return StepResult.Success;

    }
}
