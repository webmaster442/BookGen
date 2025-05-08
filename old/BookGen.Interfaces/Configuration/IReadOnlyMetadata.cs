//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces.Configuration
{
    /// <summary>
    /// Metadata informations
    /// </summary>
    public interface IReadOnlyMetadata
    {
        /// <summary>
        /// Atuhtor name
        /// </summary>
        string Author { get; }
        /// <summary>
        /// Cover Image
        /// </summary>
        string CoverImage { get; }
        /// <summary>
        /// Book title
        /// </summary>
        string Title { get; }
    }
}