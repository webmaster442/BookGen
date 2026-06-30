using Markdig.Syntax;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class QuoteBlockRenderer : TerminalObjectRenderer<QuoteBlock>
{
    protected override void Write(TerminalRenderer renderer, QuoteBlock obj)
    {
        var begin = renderer.Builder.New()
            .WithForegroundColor(renderer.RenderOptions.QuoteBlockColor)
            .WithItalic()
            .ToString();

        renderer
            .Write(begin)
            .WriteChildren(obj);

        renderer
            .WriteReset();

    }
}
