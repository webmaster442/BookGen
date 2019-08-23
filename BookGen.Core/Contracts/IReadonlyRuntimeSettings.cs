//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using System.Collections.Generic;

namespace BookGen.Core.Contracts
{
    public interface IReadonlyRuntimeSettings
    {
        FsPath OutputDirectory { get; }
        FsPath SourceDirectory { get; }
        FsPath ImageDirectory { get; }
        FsPath TocPath { get; }
        IToC TocContents { get; }
        Config Configuration { get; }
        Dictionary<string, string> MetataCache { get; }
        Dictionary<string, string> InlineImgCache { get; }
        BuildConfig CurrentBuildConfig { get; }
    }
}
