//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Terminal;
using BookGen.DomainServices;

namespace BookGen.Launcher.ViewModels.Commands;

internal sealed class StartShellCommand : ProcessCommandBase
{
    private bool Launch(string shellScript, string folder)
    {
        if (InstallStatus.IsWindowsTerminalInstalled && Properties.Settings.Default.UseWindowsTerminal)
        {
            string exe = "powershell.exe";

            if (InstallStatus.IsPsCoreInstalled)
            {
                exe = "pwsh.exe";
            }
            return RunProgram(InstallDetector.WindowsTerminalExe, $"new-tab -p \"Powershell\" --title \"BookGen shell\" --colorScheme \"{WindowsTerminalScheme.DefaultShemeName}\" {exe} -ExecutionPolicy Bypass -NoExit -File \"{shellScript}\" \"{folder}\"");

        }
        else if (InstallStatus.IsPsCoreInstalled)
        {
            return RunProgram(InstallDetector.PowershellCoreExe, $"-NoExit -File \"{shellScript}\" \"{folder}\"");
        }
        else
        {
            return RunProgram(PowershellExe, $"-ExecutionPolicy Bypass -NoExit -File \"{shellScript}\" \"{folder}\"");
        }
    }

    public override bool CanExecute(string? folder)
    {
        return !string.IsNullOrEmpty(folder)
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

        string shellScript = Path.Combine(AppContext.BaseDirectory, "BookGenShell.ps1");
        if (!File.Exists(shellScript))
        {
            Message(Properties.Resources.ShellScriptWriteFail, MessageBoxImage.Error);
            return;
        }

        if (!Launch(shellScript, folder))
        {
            Message(Properties.Resources.ShellScriptStartFail, MessageBoxImage.Error);
        }
    }
}
