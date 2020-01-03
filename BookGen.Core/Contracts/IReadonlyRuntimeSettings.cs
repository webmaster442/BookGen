//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts.Configuration;
using System.Collections.Generic;

namespace BookGen.Core.Contracts
{
    /// <summary>
    /// Current Workflows Runtime settings
    /// </summary>
    public interface IReadonlyRuntimeSettings
    {
        /// <summary>
        /// Output directory path
        /// </summary>
        FsPath OutputDirectory { get; }
        /// <summary>
        /// source directory path
        /// </summary>
        FsPath SourceDirectory { get; }
        /// <summary>
        /// Image directory Path
        /// </summary>
        FsPath ImageDirectory { get; }
        /// <summary>
        /// Table of Contents parsed
        /// </summary>
        IToC TocContents { get; }
        /// <summary>
        /// Current configuration
        /// </summary>
        IReadOnlyConfig Configuration { get; }
        /// <summary>
        /// Current Metadata
        /// </summary>
        IReadOnlyDictionary<string, string> MetataCache { get; }
        /// <summary>
        /// Inline Image cache. Key is Image path, value is Image as Base64 encoded
        /// </summary>
        IReadOnlyDictionary<string, string> InlineImgCache { get; }
        /// <summary>
        /// Currently used/active build config
        /// </summary>
        IReadOnlyBuildConfig CurrentBuildConfig { get; }
    }
}
