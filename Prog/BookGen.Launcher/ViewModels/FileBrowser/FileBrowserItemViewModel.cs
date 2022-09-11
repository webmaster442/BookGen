namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal class FileBrowserItemViewModel
    {
        public string FullPath { get; init; }
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
