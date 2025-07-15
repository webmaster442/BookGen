using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.JsInterop;

namespace Bookgen.Lib.Markdown;

public sealed class RenderSettings : IDisposable
{
    private readonly IImgService _imgService;

    public required string? HostUrl { get; init; }
    public required PrismJsInterop? PrismJsInterop { get; init; }
    public required CssClasses CssClasses { get; init; }
    public required bool DeleteFirstH1 { get; init; }
    public int OffsetHeadingsBy { get; init; } = 0;

    public required bool AutoEmbedSupportedLinks { get; init; }

    public string RequestImage(string url)
    {
        var img = _imgService.GetImageEmbedData(url);
        return ImageUrlRewriter(img);
    }

    public Func<ImageResult, string> ImageUrlRewriter { get; init; }

    private string EmbedImage(ImageResult arg)
    {
        return (arg.ImageType == ImageType.Svg)
            ? arg.Data 
            : $"data:{arg.ImageType.GetMimeType()};base64,{arg.Data}";
    }

    public RenderSettings(IImgService imgService)
    {
        ImageUrlRewriter = EmbedImage;
        _imgService = imgService;
    }

    public void Dispose()
    {
        PrismJsInterop?.Dispose();
    }
}
