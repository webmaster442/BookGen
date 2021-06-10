//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;
using System;
using System.IO;
using System.Windows;

namespace BookGen.Launch.Launcher
{
    internal class Launcher
    {
        private readonly string _appdir;
        private string _selectedFolder;

        public enum LaunchResult
        {
            FolderNoLongerExists = 1,
            NoFolderSelected = 2,
            ShellScriptWriteFail = 3,
            ShellScriptStartFail = 4,
            Ok = 0
        }

        internal Launcher()
        {
            _appdir = AppContext.BaseDirectory;
            _selectedFolder = string.Empty;
        }

        private static void Message(string text, MessageBoxImage icon)
        {
            MessageBox.Show(text, icon.ToString(), MessageBoxButton.OK, icon);
        }

        internal (LaunchResult result, string Finalfolder) Run(bool useWinTerminal, string folder = "")
        {
            if (!string.IsNullOrEmpty(folder))
            {
                if (Directory.Exists(folder))
                {
                    _selectedFolder = folder;
                }
                else
                {
                    Message(Properties.Resources.FolderNoLongerExists, MessageBoxImage.Information);
                    return (LaunchResult.FolderNoLongerExists, string.Empty);
                }
            }
            else if (!TryselectFolder(out _selectedFolder))
            {
                Message(Properties.Resources.NoFolderSelected, MessageBoxImage.Information);
                return (LaunchResult.NoFolderSelected, string.Empty);
            }

            string _shellScript;
            if (!TryCreateShellScript(out _shellScript))
            {
                Message(Properties.Resources.ShellScriptWriteFail, MessageBoxImage.Error);
                return (LaunchResult.ShellScriptWriteFail, string.Empty);
            }

            if (!TerminalLauncher.Launch(_shellScript, useWinTerminal))
            {
                Message(Properties.Resources.ShellScriptStartFail, MessageBoxImage.Error);
                return (LaunchResult.ShellScriptStartFail, string.Empty);
            }

            return (LaunchResult.Ok, _selectedFolder);
        }

        private static bool TryselectFolder(out string folderPath)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
            {
                Description = Properties.Resources.FolderselectDescription,
                UseDescriptionForTitle = true,
                ShowNewFolderButton = false,
            };
            if (dialog.ShowDialog() == true)
            {
                folderPath = dialog.SelectedPath;
                return true;
            }
            else
            {
                folderPath = string.Empty;
                return false;
            }
        }

        private bool TryCreateShellScript(out string shellScriptPath)
        {
            try
            {
                var name = Path.GetTempFileName();
                name = Path.ChangeExtension(name, ".ps1");

                using (var file = File.CreateText(name))
                {
                    var dn = ResourceHandler.GetResourceFile<KnownFile>("Powershell/completer.dn.ps1");
                    var bg = ResourceHandler.GetResourceFile<KnownFile>("Powershell/completer.ps1");

                    file.WriteLine("$env:Path += \";{0}\"", _appdir);
                    file.WriteLine("Set-Location \"{0}\"", _selectedFolder);
                    file.WriteLine(dn);
                    file.WriteLine(bg);
                    file.WriteLine("Remove-Item \"{0}\"", name);
                    file.WriteLine("echo \"To get info on using bookgen type: Bookgen Help\"");
                    file.WriteLine("echo \"To get list of commands type: Bookgen SubCommands\"");
                    file.WriteLine("echo \"\"");
                }

                shellScriptPath = name;
                return true;
            }
            catch (IOException)
            {
                shellScriptPath = string.Empty;
                return false;
            }
        }
    }
}
