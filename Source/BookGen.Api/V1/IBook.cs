//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents a book with an index and chapters
/// </summary>
public interface IBook
{
    /// <summary>
    /// Index document of the book
    /// </summary>
    IDocument Index { get; }
    /// <summary>
    /// Chapters of the book
    /// </summary>
    IReadOnlyList<IChapter> Chapters { get; }
}
