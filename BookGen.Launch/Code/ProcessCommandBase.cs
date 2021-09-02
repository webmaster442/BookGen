//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

        protected readonly bool isWindowsTerminalInstalled;
        protected readonly bool isVSCodeInstalled;
        protected readonly bool isPsCoreInstalled;

        protected const string WindowsTerminalExe = "wt.exe";
        protected const string PowershellExe = "powershell.exe";
        protected const string PowershellCoreExe = "ps.exe";
        protected const string VsCodeExe = "code.cmd";

        protected ProcessCommandBase()
        {
            string[] pathDirs = Environment.GetEnvironmentVariable("path")?.Split(';') ?? Array.Empty<string>();
            foreach (var dir in pathDirs)
            {
                if (string.IsNullOrEmpty(dir)) continue;

                string? terminalExecutable = Path.Combine(dir, WindowsTerminalExe);
                string? vsCodeExecutable = Path.Combine(dir, VsCodeExe);
                string? psCoreExecutable = Path.Combine(dir, PowershellCoreExe);
                if (File.Exists(terminalExecutable))
                    isWindowsTerminalInstalled = true;

                if (File.Exists(vsCodeExecutable))
                    isVSCodeInstalled = true;

                if (File.Exists(psCoreExecutable))
                    isPsCoreInstalled = true;
            }
        }

        public bool CanExecute(object? parameter)
        {
            return CanExecute(parameter as string);
        }

        public virtual bool CanExecute(string? folder)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            Execute(parameter as string);
        }

        public void Refresh()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected static void Message(string text, MessageBoxImage icon)
        {
            MessageBox.Show(text, icon.ToString(), MessageBoxButton.OK, icon);
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
