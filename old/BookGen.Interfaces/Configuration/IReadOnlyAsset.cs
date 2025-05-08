//-----------------------------------------------------------------------------
// (c) 2020-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces.Configuration
{
    /// <summary>
    /// Represents an asset that needs to be coppied to the output directory
    /// </summary>
    public interface IReadOnlyAsset
    {
        /// <summary>
        /// Source path not containing the root source folder
        /// </summary>
        string Source { get; }
        /// <summary>
        /// Target path not containing the target root folder
        /// </summary>
        string Target { get; }
        /// <summary>
        /// If true, the asset should be minified
        /// </summary>
        bool Minify { get; }
    }
}
