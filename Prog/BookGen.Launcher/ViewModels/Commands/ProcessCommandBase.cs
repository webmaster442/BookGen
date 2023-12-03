//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Domain.Terminal;
using BookGen.DomainServices;
using BookGen.Launcher.Properties;

namespace BookGen.Launcher.ViewModels.Commands;

internal abstract class ProcessCommandBase : CommandBase
{
    protected InstallStatus InstallStatus;

    protected const string PowershellExe = "powershell.exe";

    protected ProcessCommandBase()
    {
        InstallStatus = InstallDetector.GetInstallStatus();
    }

    public override bool CanExecute(object? parameter)
    {
        return CanExecute(parameter as string);
    }

    public virtual bool CanExecute(string? folder)
    {
        return Directory.Exists(folder);
    }

    public override void Execute(object? parameter)
    {
        Execute(parameter as string);
        if (Settings.Default.AutoExitLauncher)
        {
            Settings.Default.Save();
            Application.Current.Shutdown(0);
        }
    }

    protected static void Message(string text, MessageBoxImage icon)
    {
        Dialog.ShowMessageBox(text, icon.ToString(), MessageBoxButton.OK, icon);
    }

    protected static bool RunProgram(string program, string arguments)
    {
        try
        {
            using var process = new Process();
            process.StartInfo.UseShellExecute = false;
            if (program.EndsWith(".cmd"))
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {program} {arguments}";
            }
            else
            {
                process.StartInfo.FileName = program;
                process.StartInfo.Arguments = arguments;
            }
            process.Start();

            return true;
        }
        catch (Win32Exception)
        {
            return false;
        }
    }

    public abstract void Execute(string? folder);
}
