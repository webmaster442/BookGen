//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;
using System.Text;

namespace BookGen.Gui
{
    public sealed class ConsoleProgressbar : IProgress<int>, IProgress<IEnumerable<string>>
    {
        public int Maximum { get; }
        public int Minimum { get; }

        private bool _alternateBuffer;

        public void SwitchBuffers()
        {
            if (!_alternateBuffer)
            {
                //entering
                AnsiConsole.Write("\x1b[?1049h");
            }
            else
            {
                //exiting
                AnsiConsole.Write("\x1b[?1049l");
            }
            _alternateBuffer = !_alternateBuffer;
        }

        public ConsoleProgressbar(int minumum, int maximum)
        {
            Minimum = minumum;
            Maximum = maximum;
        }

        public void Report(int value)
        {
            Report(value, string.Empty);
        }

        public void Report(int value, string message, params object[] args)
        {
            AnsiConsole.Clear();
            int range = Maximum - Minimum;
            int maxChars = Console.WindowWidth - "   ││   ".Length;
            int charcount = value * maxChars / range;
            var buffer = new StringBuilder();

            buffer.Append("   │");
            for (int i = 0; i < maxChars; i++)
            {
                if (i < charcount)
                    buffer.Append('█');
                else
                    buffer.Append(' ');
            }
            buffer.Append("│   ");

            int height = (Console.WindowHeight - 3) / 2;

            Console.SetCursorPosition(0, height);
            AnsiConsole.Write(buffer.ToString());
            AnsiConsole.Write(buffer.ToString());
            AnsiConsole.WriteLine("  " + message, args);
        }

        public void Report(IEnumerable<string> value)
        {
            foreach (string? item in value)
            {
                AnsiConsole.WriteLine(item);
            }
        }
    }
}
