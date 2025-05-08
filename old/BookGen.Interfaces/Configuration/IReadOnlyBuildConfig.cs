﻿//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces.Configuration
{
    /// <summary>
    /// Current build configuration
    /// </summary>
    public interface IReadOnlyBuildConfig
    {
        /// <summary>
        /// Config output directory
        /// </summary>
        string OutPutDirectory { get; }
        /// <summary>
        /// Template file path
        /// </summary>
        string TemplateFile { get; }
        /// <summary>
        /// List of required assets
        /// </summary>
        IReadOnlyList<IReadOnlyAsset> TemplateAssets { get; }
        /// <summary>
        /// Additional style classes that will be aplied
        /// </summary>
        IReadOnylStyleClasses StyleClasses { get; }
        /// <summary>
        /// Additional template options
        /// </summary>
        IReadOnlyTemplateOptions TemplateOptions { get; }
        /// <summary>
        /// Image processing options for this build configuration
        /// </summary>
        IReadonlyImageOptions ImageOptions { get; }
    }
}
