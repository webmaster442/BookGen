//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Contracts.Configuration
{
    public interface IReadOnlyConfig
    {
        string HostName { get; }
        string ImageDir { get;}
        string Index { get;}
        long InlineImageSizeLimit { get;}
        bool LinksOutSideOfHostOpenNewTab { get;}
        IReadOnlyMetadata Metadata { get;}
        string ScriptsDirectory { get;}
        IReadOnlyBuildConfig TargetEpub { get;}
        IReadOnlyBuildConfig TargetPrint { get;}
        IReadOnlyBuildConfig TargetWeb { get;}
        IReadOnlyBuildConfig TargetWordpress { get;}
        string TOCFile { get;}
        IReadOnlyTranslations Translations { get;}
        int Version { get;}
    }
}