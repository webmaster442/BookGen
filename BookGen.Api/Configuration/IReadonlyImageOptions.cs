//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.Configuration
{
    /// <summary>
    /// Image pipeline options
    /// </summary>
    public interface IReadonlyImageOptions
    {
        /// <summary>
        /// If set to true, then JPEG images will be converted to WebP format
        /// </summary>
        bool RecodeJpegToWebp { get; }

        /// <summary>
        /// WebP codec quality. Minimum 0 and maximum 100. Only used, when RecodeJpegToWebp enabled.
        /// </summary>
        int ImageQuality { get; }

        /// <summary>
        /// If set to true, then output images will be resized
        /// </summary>
        bool EnableResize { get; }
        /// <summary>
        /// Maximal output image width when resize enabled or image is SVG
        /// </summary>
        int MaxWidth { get; }
        /// <summary>
        /// Maximal output image height when resize enabled or image is SVG
        /// </summary>
        int MaxHeight { get; }
        /// <summary>
        /// Inline images, that are smaller the given limit in bytes
        /// </summary>
        long InlineImageSizeLimit { get; }
    }
}
