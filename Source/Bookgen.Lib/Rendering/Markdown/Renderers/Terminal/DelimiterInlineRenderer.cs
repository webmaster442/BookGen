//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class DelimiterInlineRenderer : TerminalObjectRenderer<DelimiterInline>
{
    protected override void Write(TerminalRenderer renderer, DelimiterInline obj)
    {
        renderer.Write(obj.ToLiteral());
        renderer.WriteChildren(obj);
    }
}
