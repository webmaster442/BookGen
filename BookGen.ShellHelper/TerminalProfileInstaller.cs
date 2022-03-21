//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ShellHelper.Domain;
using System;
using System.IO;

namespace BookGen.ShellHelper
{
    public static class TerminalProfileInstaller
    {
        public static void Install()
        {
            string title = "BookGen Shell";
#if DEBUG
            title = "BookGen Shell (Dev version)";
#endif
            var profile = CreateProfile(title);
        }

        private static WindowsTerminalProfile CreateProfile(string title)
        {
            return new WindowsTerminalProfile
            {
                StartingDirectory = "%userprofile%",
                Hidden = false,
                Icon = Path.Combine(AppContext.BaseDirectory, "bookgen-icon.png"),
                Name = title,
                TabTitle = title,
                CommandLine = GetCommandLine(),
            };
        }

        private static string GetCommandLine()
        {
            string shellScript = Path.Combine(AppContext.BaseDirectory, "BookGenShell.ps1");
            return $"powershell.exe -ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"";
        }
    }
}
