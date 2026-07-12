//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace BookGen.Lib.Rendering.Markdown.Keyboard;

internal sealed class KeyboardInline : LeafInline
{
    public StringSlice Text { get; set; }
}
