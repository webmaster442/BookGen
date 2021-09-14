//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections;
using Terminal.Gui;

namespace BookGen.Gui.Views
{
    internal sealed record ListBoxElement : ElementBase
    {
        public Action<ListViewItemEventArgs>? SelectedItemChanged { get; init; }
        public string Title { get; init; }
        public int SelectedIndex { get; init; }
        public IList? Source { get; init; }
        public float Width { get; init; }

        public ListBoxElement()
        {
            Title = string.Empty;
        }
    }
}
