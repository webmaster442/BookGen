//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace BookGen.DomainServices.Markdown.TableOfContents;

internal sealed class HtmlTocRenderer : HtmlObjectRenderer<TocBlock>
{
    public TocState Options { get; }

    public HtmlTocRenderer(TocState options)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    protected override void Write(HtmlRenderer renderer, TocBlock obj)
    {
        if (Options.Headings.Count < 1)
            return;
        renderer.EnsureLine();

        var attr = obj.GetAttributes();
        if (attr.Id is null)
            attr.Id = "toc";

        if (renderer.EnableHtmlForBlock)
        {
            renderer.Write("<nav")
                .WriteAttributes(obj)
                .Write(">");
        }
        WriteTitle(renderer, obj);

        if (renderer.EnableHtmlForBlock)
        {
            Options.Headings.RenderHtml(renderer, Options);
            renderer.Write($"</nav>");
        }
        renderer.EnsureLine();
    }

    private static void WriteTitle(HtmlRenderer renderer, TocBlock obj)
    {
        if (obj.Inline?.Any() == false)
            return;

        if (renderer.EnableHtmlForBlock)
        {
            renderer.Write("<h1 class=\"toc-title\">");
        }

        renderer.WriteLeafInline(obj);

        if (renderer.EnableHtmlForBlock)
        {
            renderer.Write("</h1>");
        }
    }
}
