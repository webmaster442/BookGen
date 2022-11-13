using BookGen.Domain.Configuration;
using BookGen.Interfaces;

namespace BookGen.ProjectHandling
{
    internal sealed class LoadState
    {
        public FsPath WorkDir { get; }
        public Config? Config { get; set; }

        public ConfigFormat ConfigFormat { get; set; }
        public ToC? Toc { get; set; }
        public Dictionary<string, string[]> Tags { get; set; }

        public LoadState(string workDir)
        {
            ConfigFormat = ConfigFormat.Json;
            Tags = new Dictionary<string, string[]>();
            WorkDir = new(workDir);
        }
    }
}
