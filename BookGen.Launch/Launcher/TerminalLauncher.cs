//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace BookGen.Launch.Launcher
{
    internal static class TerminalLauncher
    {
        private const string WindowsTerminalCommand = "wt.exe";
        private const string Powershell = "powershell.exe";

        private static bool IsWindowsTerminalInstalled()
        {
            string[] pathDirs = Environment.GetEnvironmentVariable("path")?.Split(';') ?? Array.Empty<string>();
            foreach (var dir in pathDirs)
            {
                if (string.IsNullOrEmpty(dir)) continue;

                var terminalExecutable = Path.Combine(dir, WindowsTerminalCommand);
                if (File.Exists(terminalExecutable))
                {
                    return true;
                }

            }
            return false;
        }

        private static bool RunProgram(string program, string arguments)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = program;
                process.StartInfo.Arguments = arguments;
                process.Start();

                return true;
            }
            catch (Win32Exception)
            {
                return false;
            }
        }

        public static bool Launch(string shellScript, bool useWinTerminal)
        {
            if (IsWindowsTerminalInstalled() && useWinTerminal)
                return RunProgram(WindowsTerminalCommand, $"new-tab -p \"Powershell\" --title \"BookGen shell\" powershell.exe -ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"");
            else
                return RunProgram(Powershell, $"-ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"");
        }
    }
}
