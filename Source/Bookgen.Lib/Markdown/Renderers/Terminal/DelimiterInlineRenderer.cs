using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

internal sealed class DelimiterInlineRenderer : TerminalObjectRenderer<DelimiterInline>
{
    protected override void Write(TerminalRenderer renderer, DelimiterInline obj)
    {
        renderer.Write(obj.ToLiteral());
        renderer.WriteChildren(obj);
    }
}