using System.ComponentModel.DataAnnotations;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class ImageConfig
{
    public SvgRecodeOption SvgRecode { get; init; }

    [Range(-1, int.MaxValue)]
    public int ResizeWith { get; init; }

    [Range(-1, int.MaxValue)]
    public int ResizeHeight { get; init; }

    public bool ResizeAndRecodeImagesToWebp { get; init; }

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
