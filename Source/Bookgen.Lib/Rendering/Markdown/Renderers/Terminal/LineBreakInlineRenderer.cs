//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class LineBreakInlineRenderer : TerminalObjectRenderer<LineBreakInline>
{
    protected override void Write(TerminalRenderer renderer, LineBreakInline obj)
    {
        if (obj.IsHard)
        {
            renderer.WriteLine();
        }
        renderer.EnsureLine();
    }
}
