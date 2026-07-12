using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace BookGen.Lib.Rendering.Markdown.Keyboard;

internal sealed class KeyboardInline : LeafInline
{
    public StringSlice Text { get; set; }
}
