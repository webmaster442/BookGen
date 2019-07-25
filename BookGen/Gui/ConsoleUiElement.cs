//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Renderering;

namespace BookGen.Gui
{
    internal abstract class ConsoleUiElement
    {
        public Color Background { get; set; }

        public Color Foreground { get; set; }

        protected ConsoleUiElement()
        {
            Background = Color.Black;
            Foreground = Color.White;
        }

        public abstract void Render(ITerminalRenderer target);
    }
}
