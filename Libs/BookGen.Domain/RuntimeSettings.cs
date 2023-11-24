//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Concurrent;

using BookGen.Domain.Configuration;
using BookGen.Interfaces;
using BookGen.Interfaces.Configuration;

namespace BookGen.Domain
{
    public sealed class RuntimeSettings : IReadonlyRuntimeSettings
    {
        public FsPath OutputDirectory { get; set; }
        public FsPath SourceDirectory { get; init; }
        public FsPath ImageDirectory { get; set; }
        public ITableOfContents TocContents { get; init; }
        public Config Configuration { get; init; }
        public Dictionary<string, string> MetataCache { get; init; }
        public ConcurrentDictionary<string, string> InlineImgCache { get; init; }
        public BuildConfig CurrentBuildConfig { get; init; }

        IReadOnlyConfig IReadonlyRuntimeSettings.Configuration => Configuration;

        IDictionary<string, string> IReadonlyRuntimeSettings.MetataCache => MetataCache;

        IDictionary<string, string> IReadonlyRuntimeSettings.InlineImgCache => InlineImgCache;

        IReadOnlyBuildConfig IReadonlyRuntimeSettings.CurrentBuildConfig => CurrentBuildConfig;

        public ITagUtils Tags { get; }

        public RuntimeSettings(ITagUtils tags)
        {
            OutputDirectory = FsPath.Empty;
            SourceDirectory = FsPath.Empty;
            ImageDirectory = FsPath.Empty;
            TocContents = new ToC();
            Configuration = new Config();
            MetataCache = new Dictionary<string, string>();
            InlineImgCache = new ConcurrentDictionary<string, string>();
            CurrentBuildConfig = new BuildConfig();
            Tags = tags;
        }
    }
}
