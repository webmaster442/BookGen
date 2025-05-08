//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
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
        public required FsPath SourceDirectory { get; init; }
        public FsPath ImageDirectory { get; set; }
        public required ITableOfContents TocContents { get; init; }
        public required Config Configuration { get; init; }
        public Dictionary<string, string> MetataCache { get; init; }
        public ConcurrentDictionary<string, string> InlineImgCache { get; init; }
        public required BuildConfig CurrentBuildConfig { get; init; }

        IReadOnlyConfig IReadonlyRuntimeSettings.Configuration => Configuration;

        IDictionary<string, string> IReadonlyRuntimeSettings.MetataCache => MetataCache;

        IDictionary<string, string> IReadonlyRuntimeSettings.InlineImgCache => InlineImgCache;

        IReadOnlyBuildConfig IReadonlyRuntimeSettings.CurrentBuildConfig => CurrentBuildConfig;

        public required ITagUtils Tags { get; init; }

        public RuntimeSettings()
        {
            OutputDirectory = FsPath.Empty;
            ImageDirectory = FsPath.Empty;
            MetataCache = [];
            InlineImgCache = new ConcurrentDictionary<string, string>();
        }
    }
}
