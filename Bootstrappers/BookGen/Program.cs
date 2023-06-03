//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.IO;

using Bookgen.Win;

namespace BookGen
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            string shellExe = InstallDetector.IsInstalled(Constants.PowershellCore)
                ? Constants.PowershellCore 
                : Constants.Powershell;

            string shellScript = Path.Combine(AppContext.BaseDirectory, Constants.DataFolder, "BookGenShell.ps1");

            ExceptionHandler.Try(() =>
            {
                var processBuilder = new ProcessBuilder();

                if (InstallDetector.IsInstalled(Constants.WindowsTerminal))
                {
                    StartTerminal(shellExe, shellScript, processBuilder);
                }
                else
                {
                    StartPowershell(shellExe, shellScript, processBuilder);
                }
            });

        }

        private static void StartPowershell(string shellExe, string shellScript, ProcessBuilder processBuilder)
        {
            processBuilder
                .SetProgram(shellExe)
                .SetWorkDir(AppContext.BaseDirectory)
                .SetArguments(new[]
                {
                            "-ExecutionPolicy",
                            "Bypass",
                            "-NoExit",
                            "-File",
                            shellScript,
                            AppContext.BaseDirectory,
                })
                .Build()
                .Start();
        }

        private static void StartTerminal(string shellExe, string shellScript, ProcessBuilder processBuilder)
        {
            processBuilder
                .SetProgram(Constants.WindowsTerminal)
                .SetArguments(new[] {
                            "new-tab",
                            "-p",
                            "Powershell",
                            "--title",
                            "BookGen shell",
                            shellExe,
                            "-ExecutionPolicy",
                            "Bypass",
                            "-NoExit",
                            "-File",
                            shellScript,
                            AppContext.BaseDirectory,
                })
                .SetWorkDir(AppContext.BaseDirectory)
                .Build()
                .Start();
        }
    }
}