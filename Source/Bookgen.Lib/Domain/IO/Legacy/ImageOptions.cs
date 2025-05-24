//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Domain.IO.Legacy;

public sealed class ImageOptions
{
    public bool RecodeJpegToWebp { get; set; }

    public bool EnableResize { get; set; }

    public int MaxWidth { get; set; }

    public int MaxHeight { get; set; }

    public long InlineImageSizeLimit { get; set; }

    public int ImageQuality { get; set; }

    public bool RecodePngToWebp { get; set; }

    public bool EncodeSvgAsWebp { get; set; }

    public bool SvgPassthru { get; set; }

    public ImageOptions()
    {
        EnableResize = true;
        MaxWidth = 1080;
        MaxHeight = 1080;
        InlineImageSizeLimit = 0;
        ImageQuality = 95;
    }
}
