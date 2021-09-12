//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui.Views
{
    internal record ElementBase
    {
        public int Left { get; init; }
        public int Top { get; init; }

        public float Width { get; init; }

        public WidthHandling WidthHandling { get; init; }

        public ElementBase()
        {
            WidthHandling = WidthHandling.Auto;
        }
    }
}
