//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Syntax;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class ParagraphRenderer : TerminalObjectRenderer<ParagraphBlock>
{
    protected override void Write(TerminalRenderer renderer, ParagraphBlock obj)
    {
        renderer
            .WriteLeafInline(obj)
            .WriteLine()
            .WriteLine();
    }
}
