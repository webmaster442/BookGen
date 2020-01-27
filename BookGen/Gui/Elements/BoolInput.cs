//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Renderering;

namespace BookGen.Gui.Elements
{
    internal class BoolInput : ConsoleUiElement, IHaveContent
    {
        public string Content { get; set; }

        public bool Value { get; set; }

        public BoolInput()
        {
            Content = string.Empty;
            Foreground = Color.Green;
        }

        public override void Render(ITerminalRenderer target)
        {
            target.Text(Content, Foreground, Background, TextFormat.Default);
            target.Text(" [Y/N]: ", Foreground, Background, TextFormat.BoldBright);
            char input = target.ReadChar();
            Value = input == 'Y' || input == 'y';
        }
    }
}
