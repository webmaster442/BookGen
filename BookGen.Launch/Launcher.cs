//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows.Forms;

namespace BookGen.Launch
{
    internal class Launcher
    {
        private readonly string _appdir;
        private string _selectedFolder;
        private string _shellScript;

        internal Launcher()
        {
            _appdir = AppContext.BaseDirectory;
            _selectedFolder = string.Empty;
            _shellScript = string.Empty;
        }

        internal void Run()
        {
            if (!TryselectFolder(out _selectedFolder))
            {
                Message("No folder was selected. Application will exit.", MessageBoxIcon.Information);
                Environment.Exit(1);
            }

            if (!TryCreateShellScript(out _shellScript))
            {
                Message("Failed to write start script. Application will exit.", MessageBoxIcon.Error);
                Environment.Exit(2);
            }

            if (!TerminalLauncher.Launch(_shellScript))
            {
                Message("Failed to start shell. Application will exit.", MessageBoxIcon.Error);
                Environment.Exit(3);
            }
        }

        private static void Message(string text, MessageBoxIcon icon)
        {
            MessageBox.Show(text, icon.ToString(), MessageBoxButtons.OK, icon);
        }

        private static bool TryselectFolder(out string folderPath)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Select a folder to start bookgen shell",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = false,
                AutoUpgradeEnabled = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
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
                    file.WriteLine("$env:Path += \";{0}\"", _appdir);
                    file.WriteLine("Set-Location \"{0}\"", _selectedFolder);
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
