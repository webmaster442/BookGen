//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Text;

namespace BookGen.Gui.Renderering
{
    internal class VT100Renderer : ITerminalRenderer
    {
        private const string ESC = "\u001b";
        private const string BEL = "\x07";
        private const string FormatStart = "\x1b[";
        private const string FormatEnd = "m";
        private const string TitleFormat = ESC + "]2;{0}" + BEL; //ESC ] 2 ; <string> BEL
        private const string ClearFormat = ESC + "[2J" + ESC + "[;H";

        public void Clear()
        {
            Console.Write(ClearFormat);
        }

        public void DisplayError(string msg)
        {
            Text(msg, Color.Red, Color.Black, TextFormat.BoldBright);
            Text("", Color.White, Color.Black, TextFormat.Default);
        }

        public int GetInputChoice()
        {
            Text("\r\nEnter Choice", Color.Red, Color.Black, TextFormat.BoldBright);
            Text(": ", Color.White, Color.Black, TextFormat.Default);
            var str = Console.ReadLine();
            if (int.TryParse(str, out int value))
            {
                return value;
            }
            return -1;
        }

        public void NewLine()
        {
            Console.Write("\r\n");
        }

        public void PressKeyContinue()
        {
            Text("Press ENTER to continue...", Color.Green, Color.Black, TextFormat.BoldBright);
            Text("", Color.White, Color.Black, TextFormat.Default);
            Console.ReadLine();
        }

        public void SetWindowTitle(string title)
        {
            Console.WriteLine(TitleFormat, title);
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
    }
}
