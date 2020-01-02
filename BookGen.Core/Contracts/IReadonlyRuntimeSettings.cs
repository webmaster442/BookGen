//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts.Configuration;
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
        IReadOnlyConfig Configuration { get; }
        IReadOnlyDictionary<string, string> MetataCache { get; }
        IReadOnlyDictionary<string, string> InlineImgCache { get; }
        IReadOnlyBuildConfig CurrentBuildConfig { get; }
    }
}
