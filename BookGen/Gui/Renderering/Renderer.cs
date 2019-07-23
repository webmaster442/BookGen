//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BookGen.Gui.Renderering
{
    //https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
    //http://ascii-table.com/ansi-escape-sequences-vt-100.php
    internal class Renderer
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        private const string ESC = "\u001b";
        private const string BEL = "\x07";
        private const string FormatStart = "\x1b[";
        private const string FormatEnd = "m";
        private const string TitleFormat = ESC+"]2;{0}"+BEL; //ESC ] 2 ; <string> BEL
        private const string ClearFormat = ESC + "[2J" + ESC + "[;H";

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);


        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        public Renderer()
        {
            EnableVTConsoleIfWin10();
        }

        private void EnableVTConsoleIfWin10()
        {
            var versionString = RuntimeInformation.OSDescription.Replace("Microsoft Windows ", "");
            if (Environment.OSVersion.Platform == PlatformID.Win32NT
                && Version.TryParse(versionString, out Version OsVersion)
                && OsVersion >= new Version(10, 0, 1511))
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
            else
            {
                Console.WriteLine("Windows 10 build 1511 is at least required for GUI");
                Environment.Exit(-2);
            }
        }

        public void Text(string text, 
                         Color foreground, 
                         Color background,
                         TextFormat format,
                         params object[] arguments)
        {
            StringBuilder combined = new StringBuilder(80);
            combined.Append(FormatStart);
            combined.Append(foreground.GetForeground());
            combined.Append(";");
            combined.Append(background.GetBackgound());
            combined.Append(";");
            combined.Append(((int)format).ToString());
            combined.Append(FormatEnd);
            combined.Append(text);
            Console.Write(combined.ToString(), arguments);
        }

        public void DisplayError(string msg)
        {
            Text(msg, Color.Red, Color.Black, TextFormat.BoldBright);
            Text("", Color.White, Color.Black, TextFormat.Default);
        }

        public void PressKeyContinue()
        {
            Text("Press a key to continue...", Color.Green, Color.Black, TextFormat.BoldBright);
            Text("", Color.White, Color.Black, TextFormat.Default);
            Console.ReadKey();
        }

        public void NewLine()
        {
            Console.Write("\r\n");
        }

        public void Clear()
        {
            Console.Write(ClearFormat);
        }

        public void SetWindowTitle(string title)
        {
            Console.WriteLine(TitleFormat, title);
        }
    }
}
