using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

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