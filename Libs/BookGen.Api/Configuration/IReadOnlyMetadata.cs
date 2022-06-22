//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.Configuration
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