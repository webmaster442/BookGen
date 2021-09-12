//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui.Views
{
    internal record TextElement : ElementBase
    {
        public string Text { get; init; }

        public TextElement()
        {
            Text = string.Empty;
        }
    }
}
