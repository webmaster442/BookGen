//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace BookGen.Gui.Renderering
{
    internal static class NativeWrapper
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();

        private static bool IsWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }

        private static bool IsNewerThanWindows10Build1511()
        {
            var versionString = RuntimeInformation.OSDescription.Replace("Microsoft Windows ", "");
            return Version.TryParse(versionString, out Version OsVersion)
                    && OsVersion >= new Version(10, 0, 1511);
        }

        private static bool IsInTerminalEmulator()
        {
            var terminal = Environment.GetEnvironmentVariable("TERM");
            return !string.IsNullOrEmpty(terminal);
        }

        private static void EnableVt100Functions()
        {
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Environment.Exit(-2);
            }
            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                Environment.Exit(-2);
            }
        }

        public static ITerminalRenderer GetRenderer()
        {
            if (IsWindows())
            {
                if (IsInTerminalEmulator())
                {
                    return new VT100Renderer();
                }
                else if (IsNewerThanWindows10Build1511())
                {
                    EnableVt100Functions();
                    return new VT100Renderer();
                }
                else
                {
                    return new DumbRenderer();
                }
            }
            else if (IsInTerminalEmulator())
            {
                return new VT100Renderer();
            }
            else
            {
                return new DumbRenderer();
            }
        }
    }
}
