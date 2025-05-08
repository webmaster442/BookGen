//-----------------------------------------------------------------------------
// (c) 2020  Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces.Configuration
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
        /// If set to true, then PNG images will be converted to WebP format
        /// </summary>
        bool RecodePngToWebp { get; }

        /// <summary>
        /// If set to true, then SVG images will be converted to WebP format
        /// </summary>
        bool EncodeSvgAsWebp { get; }

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

        /// <summary>
        /// If set to true, then SVG images will be passed thrue, without reencoding
        /// </summary>
        bool SvgPassthru { get; set; }
    }
}
