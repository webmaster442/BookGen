using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class LinkInlineRenderer : TerminalObjectRenderer<LinkInline>
{
    protected override void Write(TerminalRenderer renderer, LinkInline obj)
    {
        if (obj.IsImage)
        {
            // TODO
            return;
        }

        string? linkText = obj.FirstChild?.ToString();

        if (obj.Url is null
            || linkText is null)
        {
            return;
        }

        string text = renderer
            .Builder
            .New()
            .WithForegroundColor(renderer.RenderOptions.LinkColor)
            .AppendLink(obj.Url, linkText)
            .ResetFormat()
            .ToString();

        renderer.Write(text);

    }
}
