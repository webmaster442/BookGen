//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal sealed class FileBrowserTreeViewModel
    {
        public string Name { get; init; }
        public string FullPath { get; init; }

        public FileBrowserTreeViewModel[] SubItems { get; init; }

        public FileBrowserTreeViewModel()
        {
            Name = string.Empty;
            FullPath = string.Empty;
            SubItems = Array.Empty<FileBrowserTreeViewModel>();
        }
    }
}
