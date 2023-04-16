//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels;

internal sealed record ItemViewModel
{
    public string FolderName
    {
        get
        {
            if (string.IsNullOrEmpty(FullPath))
                return string.Empty;

            string folderName = Path.GetFileName(FullPath);

            if (string.IsNullOrEmpty(folderName))
                return FullPath;

            return folderName;
        }
    }

    public bool IsDisabled => !Directory.Exists(FullPath);

    public bool IsEnabled => Directory.Exists(FullPath);

    public string FullPath { get; init; }

    public ItemViewModel()
    {
        FullPath = string.Empty;
    }
}
