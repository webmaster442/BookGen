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

        internal Launcher()
        {
            _appdir = AppContext.BaseDirectory;
            _selectedFolder = string.Empty;
        }

        private static void Message(string text, MessageBoxImage icon)
        {
            MessageBox.Show(text, icon.ToString(), MessageBoxButton.OK, icon);
        }

        internal void Run(bool useWinTerminal)
        {
            if (!TryselectFolder(out _selectedFolder))
            {
                Message("No folder was selected. Application will exit.", MessageBoxImage.Information);
                Environment.Exit(1);
            }

            string _shellScript;
            if (!TryCreateShellScript(out _shellScript))
            {
                Message("Failed to write start script. Application will exit.", MessageBoxImage.Error);
                Environment.Exit(2);
            }

            if (!TerminalLauncher.Launch(_shellScript, useWinTerminal))
            {
                Message("Failed to start shell. Application will exit.", MessageBoxImage.Error);
                Environment.Exit(3);
            }
        }

        private static bool TryselectFolder(out string folderPath)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select a folder to start bookgen shell",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = false,
                AutoUpgradeEnabled = true
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
