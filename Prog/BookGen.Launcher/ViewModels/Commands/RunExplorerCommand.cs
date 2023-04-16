//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels.Commands;

internal sealed class RunExplorerCommand : ProcessCommandBase
{
    public override void Execute(string? folder)
    {
        if (string.IsNullOrEmpty(folder)
            || !Directory.Exists(folder))
        {
            Message(Properties.Resources.FolderNoLongerExists, MessageBoxImage.Error);
            return;
        }

        RunProgram("explorer.exe", folder);
    }
}
