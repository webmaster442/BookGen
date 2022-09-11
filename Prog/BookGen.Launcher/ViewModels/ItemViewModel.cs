//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels
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
