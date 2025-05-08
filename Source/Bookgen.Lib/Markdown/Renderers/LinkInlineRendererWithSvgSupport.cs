using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Markdown.Renderers;

internal sealed class LinkInlineRendererWithSvgSupport : LinkInlineRenderer
{
    protected override void Write(HtmlRenderer renderer, LinkInline obj)
    {
        if (obj.IsImage
            && obj.Url?.Contains("<svg") == true
            && obj.Url?.Contains("</svg>") == true)
        {
            renderer.Write(obj.Url);
            return;
        }

        base.Write(renderer, obj);
    }
}