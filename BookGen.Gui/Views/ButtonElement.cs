//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Gui.Views
{
    internal record ButtonElement : TextElement
    {
        public Action? OnClick { get; init; }
    }
}
