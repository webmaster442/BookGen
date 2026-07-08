//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents options for configuring the renderer used in the book generation process.
/// </summary>
public sealed class RendererOptions
{
    /// <summary>
    /// Specifies how images should be processed during the rendering process.
    /// </summary>
    public enum ImageOption
    {
        /// <summary>
        /// Indicates that images should be passed through without any modifications.
        /// </summary>
        Passtrough = 0,
        /// <summary>
        /// Indicates that images should be converted to PNG format during the rendering process.
        /// </summary>
        AsPng = 1,
        /// <summary>
        /// Indicates that images should be converted to WebP format during the rendering process.
        /// </summary>
        AsWebp = 2,
    }

    /// <summary>
    /// Specifies whether the first H1 heading in the content should be deleted during the rendering process.
    /// </summary>
    public bool DeleteFirstH1 { get; set; }

    /// <summary>
    /// Specifies the base URL of the host where the book will be published. This is used for generating absolute URLs for links and resources in the rendered content.
    /// </summary>
    public string HostUrl { get; set; } = string.Empty;

    /// <summary>
    /// Specifies whether supported links should be automatically embedded in the rendered content.
    /// If set to true, links to supported content types (e.g., images, videos) will be embedded directly in the output.
    /// </summary>
    public bool AutoEmbedSupportedLinks { get; set; } = true;

    /// <summary>
    /// Specifies the width to which images should be resized during the rendering process.
    /// A value of -1 indicates that the original width should be preserved.
    /// </summary>
    public int ResizeWidth { get; set; } = -1;
    
    /// <summary>
    /// Specifies the height to which images should be resized during the rendering process.
    /// A value of -1 indicates that the original height should be preserved.
    /// </summary>
    public int ResizeHeight { get; set; } = -1;

    /// <summary>
    /// Specifies how SVG images should be processed during the rendering process.
    /// </summary>
    public ImageOption SvgRecode { get; set; } = ImageOption.Passtrough;

    /// <summary>
    /// Specifies how raster images (e.g., JPEG, PNG) should be processed during the rendering process.
    /// </summary>
    public ImageOption ImageRecode { get; set; } = ImageOption.Passtrough;

    /// <summary>
    /// Specifies the number of heading levels by which to offset headings in the rendered content.
    /// </summary>
    public CssClasses CssClasses { get; set; } = new CssClasses();
}
