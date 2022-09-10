using System;

namespace BookGen.Launcher.ViewModels
{
    internal class FileBrowserItemViewModel
    {
        public string FullPath { get; init; }
        public bool IsFolder { get; init; }
        public DateTime ModificationDate { get; init; }
        public long Size { get; init; }
        public string Extension { get; init; }
        public string Name { get; init; }

        public FileBrowserItemViewModel()
        {
            Name = string.Empty;
            FullPath = string.Empty;
            Extension = string.Empty;
        }
    }
}
