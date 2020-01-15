//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;

namespace BookGen.Api
{
    /// <summary>
    /// Interface for accesing the current script runtime
    /// </summary>
    public interface IScriptHost
    {
        /// <summary>
        /// Source directory of input files
        /// </summary>
        string SourceDirectory { get; }
        /// <summary>
        /// Current configuration
        /// </summary>
        IReadOnlyConfig Configuration { get; }
        /// <summary>
        /// Currently processed book table of contents
        /// </summary>
        ITableOfContents TableOfContents { get; }
        /// <summary>
        /// Currently active build configuration
        /// </summary>
        IReadOnlyBuildConfig CurrentBuildConfig { get; }
        /// <summary>
        /// Script host log
        /// </summary>
        ILog Log { get; }
    }
}
