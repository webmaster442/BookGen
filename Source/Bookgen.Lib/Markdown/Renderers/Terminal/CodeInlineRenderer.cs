using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

internal sealed class CodeInlineRenderer : TerminalObjectRenderer<CodeInline>
{
    protected override void Write(TerminalRenderer renderer, CodeInline obj)
    {
        var begin = renderer.Builder
            .New()
            .WithForegroundColor(renderer.RenderOptions.CodeInlineColor)
            .WithItalic()
            .ToString();

        renderer.Write(begin);

        renderer.Write(obj.ContentSpan);

        renderer.WriteReset();
    }
}
