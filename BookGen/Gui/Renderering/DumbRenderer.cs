//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Gui.Renderering
{
    internal class DumbRenderer : ITerminalRenderer
    {
        public void Clear()
        {
            Console.Clear();
        }

        public void DisplayError(string msg)
        {
            Console.WriteLine(msg);
        }

        public void NewLine()
        {
            Console.WriteLine();
        }

        public void PressKeyContinue()
        {
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }

        public void SetWindowTitle(string title)
        {
            //Not implemented
        }

        public void Text(string text, Color foreground, Color background, TextFormat format, params object[] arguments)
        {
            Console.Write(text, arguments);
        }

        public int GetInputChoice()
        {
            Console.Write("\r\nEnter Choice: ");
            var str = Console.ReadLine();
            if (int.TryParse(str, out int value))
            {
                return value;
            }
            return -1;
        }
    }
}
