using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class ImageConfig
{
    [Description("Svg re-encode options")]
    public SvgRecodeOption SvgRecode { get; init; }

    [Description("Resize to max width. -1 means use input width")]
    [Range(-1, int.MaxValue)]
    public int ResizeWith { get; init; }

    [Description("Resize to max height. -1 means use input height")]
    [Range(-1, int.MaxValue)]
    public int ResizeHeight { get; init; }

    [Description("Enables or disables Webp reencode & resize")]
    public bool ResizeAndRecodeImagesToWebp { get; init; }

    [Description("WebP compression quality level")]
    [Range(1, 100)]
    public int WebpQuality { get; init; }

    public ImageConfig()
    {
        WebpQuality = 90;
        SvgRecode = SvgRecodeOption.Passtrough;
        ResizeAndRecodeImagesToWebp = false;
        ResizeWith = -1;
        ResizeHeight = -1;
    }
}
