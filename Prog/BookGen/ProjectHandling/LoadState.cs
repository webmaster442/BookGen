//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Interfaces;

namespace BookGen.ProjectHandling
{
    internal sealed class LoadState
    {
        public FsPath WorkDir { get; }
        public int ConfigVersion { get; }
        public Config? Config { get; set; }

        public ConfigFormat ConfigFormat { get; set; }
        public ToC? Toc { get; set; }
        public Dictionary<string, string[]> Tags { get; set; }


        public LoadState(string workDir, int configVersion)
        {
            ConfigFormat = ConfigFormat.Json;
            Tags = new Dictionary<string, string[]>();
            WorkDir = new(workDir);
            ConfigVersion = configVersion;
        }
    }
}
