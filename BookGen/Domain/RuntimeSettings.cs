//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Api.Configuration;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BookGen.Domain
{
    public class RuntimeSettings : IReadonlyRuntimeSettings
    {
        public FsPath OutputDirectory { get; set; }
        public FsPath SourceDirectory { get; set; }
        public FsPath ImageDirectory { get; set; }
        public ITableOfContents TocContents { get; set; }
        public Config Configuration { get; set; }
        public Dictionary<string, string> MetataCache { get; set; }
        public ConcurrentDictionary<string, string> InlineImgCache { get; set; }
        public BuildConfig CurrentBuildConfig { get; set; }

        IReadOnlyConfig IReadonlyRuntimeSettings.Configuration => Configuration;

        IReadOnlyDictionary<string, string> IReadonlyRuntimeSettings.MetataCache => MetataCache;

        IReadOnlyDictionary<string, string> IReadonlyRuntimeSettings.InlineImgCache => InlineImgCache;

        IReadOnlyBuildConfig IReadonlyRuntimeSettings.CurrentBuildConfig => CurrentBuildConfig;

        public RuntimeSettings()
        {
            OutputDirectory = FsPath.Empty;
            SourceDirectory = FsPath.Empty;
            ImageDirectory = FsPath.Empty;
            TocContents = new ToC();
            Configuration = new Config();
            MetataCache = new Dictionary<string, string>();
            InlineImgCache = new ConcurrentDictionary<string, string>();
            CurrentBuildConfig = new BuildConfig();
        }
    }
}
