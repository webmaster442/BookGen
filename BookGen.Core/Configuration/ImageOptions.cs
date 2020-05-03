﻿//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;

namespace BookGen.Core.Configuration
{
    public sealed class ImageOptions : IReadonlyImageOptions
    {
        [Doc("If set to true, then JPEG images will be converted to WebP format", IsOptional = true)]
        public bool RecodeJpegToWebp { get; set; }

        [Doc("If set to true, then output images will be resized", IsOptional = true)]
        public bool EnableResize { get; set; }

        [Doc("Maximal output image width when resize enabled or image is SVG", IsOptional = true)]
        public int MaxWidth { get; set; }

        [Doc("Maximal output image height when resize enabled or image is SVG", IsOptional = true)]
        public int MaxHeight { get; set; }

        [Doc("Inline images, that are smaller the given limit in bytes", IsOptional = true)]
        public long InlineImageSizeLimit { get; set; }

        [Doc("WebP codec quality. Minimum 0 and maximum 100. Only used, when RecodeJpegToWebp enabled.", IsOptional = true)]
        public int WebPQuality { get; set; }

        public ImageOptions()
        {
            EnableResize = true;
            MaxWidth = 1080;
            MaxHeight = 1080;
            InlineImageSizeLimit = 32 * 1024;
            WebPQuality = 95;
        }
    }
}
