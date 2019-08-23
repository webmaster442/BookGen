//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using System.Collections.Generic;

namespace BookGen.Domain
{
    public class RuntimeSettings : IReadonlyRuntimeSettings
    {
        public FsPath OutputDirectory { get; set; }
        public FsPath SourceDirectory { get; set; }
        public FsPath ImageDirectory { get; set; }
        public FsPath TocPath { get; set; }
        public IToC TocContents { get; set; }
        public Config Configuration { get; set; }
        public Dictionary<string, string> MetataCache { get; set; }
        public Dictionary<string, string> InlineImgCache { get; set; }
        public BuildConfig CurrentBuildConfig { get; set; }
    }
}
