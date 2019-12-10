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

        public string Name { get; set; }

        protected ConsoleUiElement()
        {
            Background = Color.Transparent;
            Foreground = Color.White;
            Name = string.Empty;
        }

        public abstract void Render(ITerminalRenderer target);
    }
}
