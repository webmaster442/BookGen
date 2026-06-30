using Markdig.Syntax;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class ThematicBreakRenderer : TerminalObjectRenderer<ThematicBreakBlock>
{
    protected override void Write(TerminalRenderer renderer, ThematicBreakBlock obj)
    {
        renderer.WriteLine(new string('-', renderer.RenderOptions.Width));
        renderer.WriteLine();
    }
}
