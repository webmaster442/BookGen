//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Syntax;

namespace BookGen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class ThematicBreakRenderer : TerminalObjectRenderer<ThematicBreakBlock>
{
    protected override void Write(TerminalRenderer renderer, ThematicBreakBlock obj)
    {
        renderer.WriteLine(new string('-', renderer.RenderOptions.Width));
        renderer.WriteLine();
    }
}
