//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Launch.Properties;
using BookGen.ShellHelper;
using BookGen.ShellHelper.Domain;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace BookGen.Launch.Code
{
    internal abstract class ProcessCommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        protected InstallStatus InstallStatus; 

        protected const string PowershellExe = "powershell.exe";

        protected ProcessCommandBase()
        {
            InstallStatus = InstallDetector.GetInstallStatus();
        }

        public bool CanExecute(object? parameter)
        {
            return CanExecute(parameter as string);
        }

        public virtual bool CanExecute(string? folder)
        {
            return Directory.Exists(folder);
        }

        public void Execute(object? parameter)
        {
            Execute(parameter as string);
            if (Settings.Default.AutoExitLauncher)
            {
                Settings.Default.Save();
                Application.Current.Shutdown(0);
            }
        }

        public void Refresh()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected static void Message(string text, MessageBoxImage icon)
        {
            MessageBoxEx.Show(text, icon.ToString(), MessageBoxButton.OK, icon);
        }

        protected static bool RunProgram(string program, string arguments)
        {
            try
            {
                Process process = new Process();
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
}
