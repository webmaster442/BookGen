//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ShellHelper.Domain;
using System;
using System.IO;

namespace BookGen.ShellHelper.Code
{
    public static class InstallDetector
    {
        public const string WindowsTerminalExe = "wt.exe";
        public const string PowershellCoreExe = "ps.exe";
        public const string VsCodeExe = "code.cmd";

        public static InstallStatus GetInstallStatus()
        {
            bool isWindowsTerminalInstalled = false;
            bool isVSCodeInstalled = false;
            bool isPsCoreInstalled = false;

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

            return new InstallStatus
            {
                IsPsCoreInstalled = isPsCoreInstalled,
                IsWindowsTerminalInstalled = isWindowsTerminalInstalled,
                IsVSCodeInstalled = isVSCodeInstalled
            };
        }
    }
}
