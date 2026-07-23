//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents a chapter in a book
/// </summary>
public interface IChapter
{
    /// <summary>
    /// Chapter title
    /// </summary>
    string Title { get; }
    /// <summary>
    /// Chapter contents
    /// </summary>
    IReadOnlyList<IDocument> Documents { get; }
}
