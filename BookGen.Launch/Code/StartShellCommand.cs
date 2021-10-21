//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;
using System;
using System.IO;
using System.Windows;

namespace BookGen.Launch.Code
{
    internal sealed class StartShellCommand : ProcessCommandBase
    {
        private bool TryCreateShellScript(string folder, out string shellScriptPath)
        {
            try
            {
#pragma warning disable S5445 // Insecure temporary file creation methods should not be used
                string? name = Path.GetTempFileName();
#pragma warning restore S5445 // Insecure temporary file creation methods should not be used
                name = Path.ChangeExtension(name, ".ps1");

                using (var file = File.CreateText(name))
                {
                    var dn = ResourceHandler.GetResourceFile<KnownFile>("Powershell/completer.dn.ps1");
                    var bg = ResourceHandler.GetResourceFile<KnownFile>("Powershell/completer.ps1");
                    var sh = ResourceHandler.GetResourceFile<KnownFile>("Powershell/shell.ps1");

                    file.WriteLine("$env:Path += \";{0}\"", AppContext.BaseDirectory);
                    file.WriteLine("Set-Location \"{0}\"", folder);
                    file.WriteLine(dn);
                    file.WriteLine(bg);
                    file.WriteLine("Remove-Item \"{0}\"", name);
                    file.WriteLine(sh);
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

        private bool Launch(string shellScript)
        {
            if (isWindowsTerminalInstalled && Properties.Settings.Default.UseWindowsTerminal)
                return RunProgram(WindowsTerminalExe, $"new-tab -p \"Powershell\" --title \"BookGen shell\" --colorScheme \"Campbell Powershell\" powershell.exe -ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"");
            else if (isPsCoreInstalled)
                return RunProgram(PowershellCoreExe, $"-NoExit -File \"{shellScript}\"");
            else
                return RunProgram(PowershellExe, $"-ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"");
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

            string _shellScript;
            if (!TryCreateShellScript(folder, out _shellScript))
            {
                Message(Properties.Resources.ShellScriptWriteFail, MessageBoxImage.Error);
                return;
            }

            if (!Launch(_shellScript))
            {
                Message(Properties.Resources.ShellScriptStartFail, MessageBoxImage.Error);
            }
        }
    }
}
