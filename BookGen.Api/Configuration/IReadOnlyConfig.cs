//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.Configuration
{
    /// <summary>
    /// Configuration
    /// </summary>
    public interface IReadOnlyConfig
    {
        /// <summary>
        /// Host name
        /// </summary>
        string HostName { get; }
        /// <summary>
        /// Images directory
        /// </summary>
        string ImageDir { get;}
        /// <summary>
        /// Index file
        /// </summary>
        string Index { get;}
        /// <summary>
        /// Inline image size in bytes
        /// </summary>
        long InlineImageSizeLimit { get;}
        /// <summary>
        /// Links that do not target the HostName open in new tabs
        /// </summary>
        bool LinksOutSideOfHostOpenNewTab { get;}
        /// <summary>
        /// Metadata information
        /// </summary>
        IReadOnlyMetadata Metadata { get;}
        /// <summary>
        /// Scripts folder
        /// </summary>
        string ScriptsDirectory { get;}
        /// <summary>
        /// Build configuration for epubs
        /// </summary>
        IReadOnlyBuildConfig TargetEpub { get;}
        /// <summary>
        /// Build configuration for Printing
        /// </summary>
        IReadOnlyBuildConfig TargetPrint { get;}
        /// <summary>
        /// Build configuration for static website
        /// </summary>
        IReadOnlyBuildConfig TargetWeb { get;}
        /// <summary>
        /// Build configuration for Wordpress export
        /// </summary>
        IReadOnlyBuildConfig TargetWordpress { get;}
        /// <summary>
        /// Table of contents file
        /// </summary>
        string TOCFile { get;}
        /// <summary>
        /// Translations
        /// </summary>
        IReadOnlyTranslations Translations { get;}
        /// <summary>
        /// Config file version
        /// </summary>
        int Version { get;}
    }
}