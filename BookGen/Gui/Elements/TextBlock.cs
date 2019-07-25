//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Renderering;

namespace BookGen.Gui.Elements
{
    internal class TextBlock : ConsoleUiElement
    {
        public string Text { get; set; }

        public override void Render(ITerminalRenderer target)
        {
            target.Text(Text, Foreground, Background, TextFormat.Default);
        }
    }
}
