using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

internal sealed class HeadingRenderer : TerminalObjectRenderer<HeadingBlock>
{
    protected override void Write(TerminalRenderer renderer, HeadingBlock obj)
    {
        string prefix = new string('#', obj.Level);

        var beginText = renderer
            .Builder
            .New()
            .WithForegroundColor(renderer.RenderOptions.HeadingColor)
            .WithBold()
            .Append(prefix)
            .Append(' ')
            .ToString();

        renderer
            .Write(beginText)
            .WriteLeafInline(obj)
            .WriteReset()
            .EnsureLine()
            .WriteLine();
    }
}
