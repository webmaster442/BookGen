//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Syntax.Inlines;

namespace BookGen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class LiteralInlineRenderer : TerminalObjectRenderer<LiteralInline>
{
    protected override void Write(TerminalRenderer renderer, LiteralInline obj)
    {
        string content = obj.Content.ToString();
        renderer.Write(content);
    }
}
