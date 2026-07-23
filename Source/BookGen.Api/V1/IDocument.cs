//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents a document in the book
/// </summary>
public interface IDocument
{
    /// <summary>
    /// File path of the document
    /// </summary>
    string FilePath { get; }
    /// <summary>
    /// Reads the content of the document and its front matter asynchronously
    /// </summary>
    /// <returns>A tuple containing the content and front matter of the document</returns>
    Task<(string content, IDocumentFrontMatter frontMatter)> ReadContent();
}
