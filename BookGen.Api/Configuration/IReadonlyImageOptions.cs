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
        /// If set to true, then output images will be resized
        /// </summary>
        bool EnableResize { get; }
        /// <summary>
        /// Maximal output image width when resize enabled
        /// </summary>
        int MaxWidth { get; }
        /// <summary>
        /// Maximal output image height when resize enabled
        /// </summary>
        int MaxHeight { get; }
        /// <summary>
        /// If set to true, then SVG files will be converted to PNG. Render size is set by the MaxWidth and MaxHeight property
        /// </summary>
        bool RenderSvgToPng { get; }
        /// <summary>
        /// Inline images, that are smaller the given limit in bytes
        /// </summary>
        long InlineImageSizeLimit { get; }
    }
}
