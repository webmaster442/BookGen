using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class LiteralInlineRenderer : TerminalObjectRenderer<LiteralInline>
{
    protected override void Write(TerminalRenderer renderer, LiteralInline obj)
    {
        string content = obj.Content.ToString();
        renderer.Write(content);
    }
}
