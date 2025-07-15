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

    public Func<string, string> RewriteImageUrl { get; init; }

    private string EmbedImage(string arg)
    {
        var image = _imgService.GetImageEmbedData(arg);
        if (image.ImageType == ImageType.Svg)
        {
            return image.Data;
        }
        else
        {
            return $"data:{image.ImageType.GetMimeType()};base64,{image.Data}";

        }
    }

    public RenderSettings(IImgService imgService)
    {
        RewriteImageUrl = EmbedImage;
        _imgService = imgService;
    }

    public void Dispose()
    {
        PrismJsInterop?.Dispose();
    }
}
