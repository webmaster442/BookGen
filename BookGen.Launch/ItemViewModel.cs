using System.IO;

namespace BookGen.Launch
{
    internal record ItemViewModel
    {
        public string FolderName => Path.GetFileName(FullPath) ?? string.Empty;
        public bool IsDisabled => !Directory.Exists(FullPath);
        public bool IsEnabled => Directory.Exists(FullPath);
        public string FullPath { get; init; }

        public ItemViewModel()
        {
            FullPath = string.Empty;
        }
    }
}
