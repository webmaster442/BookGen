//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.IO;

namespace Bookgen.Win
{
    public static  class InstallDetector
    {
        public static bool IsWindowsTerminalPortalbeInstalled(out string portablePath)
        {
            string[] searchPaths = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "terminal", Constants.WindowsTerminal),
                Path.Combine(AppContext.BaseDirectory, "wt", Constants.WindowsTerminal),
            };

            foreach (var searchPath in searchPaths) 
            {
                if (File.Exists(searchPath)) 
                {
                    portablePath = searchPath;
                    return true;
                }
            }

            portablePath = string.Empty;
            return false;
        }

        public static bool IsInstalled(string programName)
        {
            string[] pathDirs = Environment.GetEnvironmentVariable("path")?.Split(';') ?? Array.Empty<string>();
            foreach (var dir in pathDirs)
            {
                if (!Directory.Exists(dir)) continue;

                var file = Path.Combine(dir, programName);
                if (File.Exists(file))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
