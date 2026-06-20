//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Rendering.Images;
using Bookgen.Lib.Rendering.Markdown.RenderInterop;

namespace Bookgen.Lib.Rendering.Markdown;

public sealed class MarkdownRenderSettings : IDisposable
{
    private readonly IImgService _imgService;

    public required string? HostUrl { get; init; }
    public required IRenderInterop RenderInterop { get; init; }
    public required CssClasses CssClasses { get; init; }
    public required bool DeleteFirstH1 { get; init; }
    public int OffsetHeadingsBy { get; init; } = 0;

    public required bool AutoEmbedSupportedLinks { get; init; }

    public string RequestImage(string url)
    {
        ImageResult img = _imgService.GetImageEmbedData(url);
        return ImageUrlRewriter(img);
    }

    public Func<ImageResult, string> ImageUrlRewriter { get; init; }

    private string EmbedImage(ImageResult arg)
    {
        return (arg.ImageType == ImageType.Svg)
            ? arg.Data
            : $"data:{arg.ImageType.GetMimeType()};base64,{arg.Data}";
    }

    public MarkdownRenderSettings(IImgService imgService)
    {
        ImageUrlRewriter = EmbedImage;
        _imgService = imgService;
    }

    public void Dispose()
    {
        RenderInterop.Dispose();
    }
}
