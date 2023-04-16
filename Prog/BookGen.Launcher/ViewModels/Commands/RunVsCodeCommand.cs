//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;

namespace BookGen.Launcher.ViewModels.Commands;

internal sealed class RunVsCodeCommand : ProcessCommandBase
{
    public override bool CanExecute(string? folder)
    {
        return InstallStatus.IsVSCodeInstalled
            && Directory.Exists(folder);
    }

    public override void Execute(string? folder)
    {
        if (string.IsNullOrEmpty(folder)
            || !Directory.Exists(folder))
        {
            Message(Properties.Resources.FolderNoLongerExists, MessageBoxImage.Error);
            return;
        }

        RunProgram(InstallDetector.VsCodeExe, folder);
    }
}
