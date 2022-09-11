namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal class FileBrowserTreeViewModel
    {
        public string Name { get; init; }
        public string FullPath { get; init; }

        public bool IsExpanded { get; init; }

        public FileBrowserTreeViewModel[] SubItems { get; init; }

        public FileBrowserTreeViewModel()
        {
            Name = string.Empty;
            FullPath = string.Empty;
            SubItems = Array.Empty<FileBrowserTreeViewModel>();
        }
    }
}
