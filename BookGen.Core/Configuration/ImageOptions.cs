//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//----------------------------------------------------------------------------

using BookGen.Api.Configuration;

namespace BookGen.Core.Configuration
{
    public sealed class ImageOptions : IReadonlyImageOptions
    {
        [Doc("If set to true, then JPEG images will be converted to WebP format")]
        public bool RecodeJpegToWebp { get; set; }

        [Doc("If set to true, then output images will be resized")]
        public bool EnableResize { get; set; }

        [Doc("Maximal output image width when resize enabled")]
        public int MaxWidth { get; set; }

        [Doc("Maximal output image height when resize enabled")]
        public int MaxHeight { get; set; }

        [Doc("If set to true, then SVG files will be converted to PNG. Render size is set by the MaxWidth and MaxHeight property")]
        public bool RenderSvgToPng { get; set; }

        [Doc("Inline images, that are smaller the given limit in bytes")]
        public long InlineImageSizeLimit { get; set; }

        public ImageOptions()
        {
            EnableResize = true;
            MaxWidth = 1080;
            MaxHeight = 1080;
            RenderSvgToPng = true;
            InlineImageSizeLimit = 32 * 1024;
        }
    }
}
