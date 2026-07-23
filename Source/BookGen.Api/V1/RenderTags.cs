//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents the tags used for rendering HTML content.
/// </summary>
public sealed class RenderTags
{
    /// <summary>
    /// HTML document title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// HTML document content
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Host url for links and images
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Last modified date of the document
    /// </summary>
    public DateTime LastModified { get; set; } = DateTime.UnixEpoch;

    /// <summary>
    /// Additional data that can be used in the rendering process.
    /// </summary>
    public Dictionary<string, string> AdditionalData { get; set; } = new();
}
