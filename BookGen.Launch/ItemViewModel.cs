using System.IO;

namespace BookGen.Launch
{
    internal record ItemViewModel
    {
        public string FolderName => Path.GetFileName(FullPath) ?? string.Empty;
        public bool IsExisting => Directory.Exists(FullPath);
        public string FullPath { get; init; }

        public ItemViewModel()
        {
            FullPath = string.Empty;
        }
    }
}
