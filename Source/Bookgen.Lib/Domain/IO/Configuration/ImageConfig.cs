//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

    [Description("Sets Image resize and recoding behaviour")]
    public ImgRecodeOption ResizeAndRecodeImages { get; init; }

    [Description("WebP compression quality level")]
    [Range(1, 100)]
    public int ImageQualityOnResize { get; init; }

    public ImageConfig()
    {
        ImageQualityOnResize = 90;
        SvgRecode = SvgRecodeOption.Passtrough;
        ResizeAndRecodeImages = ImgRecodeOption.Passtrough;
        ResizeWith = -1;
        ResizeHeight = -1;
    }
}
