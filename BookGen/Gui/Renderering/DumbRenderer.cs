//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Linq;

namespace BookGen.Gui.Renderering
{
    internal class DumbRenderer : ITerminalRenderer
    {
        /// <inheritdoc/>
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc/>
        public void DisplayError(string msg)
        {
            Console.WriteLine(msg);
        }

        /// <inheritdoc/>
        public void NewLine()
        {
            Console.WriteLine();
        }

        /// <inheritdoc/>
        public void PressKeyContinue()
        {
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }

        /// <inheritdoc/>
        public void SetWindowTitle(string title)
        {
            //Not implemented
        }

        /// <inheritdoc/>
        public void Text(string text, Color foreground, Color background, TextFormat format, params object[] arguments)
        {
            Console.Write(text, arguments);
        }

        /// <inheritdoc/>
        public int? GetInputChoice()
        {
            Console.Write("\r\nEnter Choice: ");
            var str = Console.ReadLine();
            if (int.TryParse(str, out int value))
            {
                return value;
            }
            return null;
        }

        /// <inheritdoc/>
        public char ReadChar()
        {
            return Console.ReadLine().FirstOrDefault();
        }
    }
}
