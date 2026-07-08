//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Document front matter metadata
/// </summary>
public interface IDocumentFrontMatter
{
    /// <summary>
    /// Document title
    /// </summary>
    string Title { get; }
    /// <summary>
    /// Document tags
    /// </summary>
    IReadOnlyList<string> Tags { get; }
    /// <summary>
    /// Custom template for the document, if any
    /// </summary>
    string? Template { get; }
    /// <summary>
    /// Additional document data
    /// </summary>
    IReadOnlyDictionary<string, string> AdditionalData { get; }
}
