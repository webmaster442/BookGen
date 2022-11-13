using BookGen.Domain.Configuration;
using BookGen.Interfaces;

namespace BookGen.ProjectHandling
{
    internal record class LoadState
    {
        public FsPath WorkDir { get; }
        public Config? Config { get; set; }

        public LoadState(string workDir)
        {
            WorkDir = new(workDir);
        }
    }
}
