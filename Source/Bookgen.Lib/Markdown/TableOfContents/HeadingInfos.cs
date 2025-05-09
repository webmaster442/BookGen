//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Markdig.Renderers;

namespace Bookgen.Lib.Markdown.TableOfContents;

[DebuggerDisplay("Level: {Level} Content: {Content}")]
internal sealed class HeadingInfos : LevelList<HeadingInfos>
{
    public static HeadingInfos FromHeading(HeadingInfo info)
       => new HeadingInfos
       {
           Content = info.Content,
           Id = info.Id,
           Level = info.Level
       };

    public string Content { get; set; } = "";
    public string Id { get; set; } = "";

    public void RenderHtml(HtmlRenderer renderer, TocState options)
    {
        RenderHtmlLint(renderer, options);
    }

    void RenderHtmlLint(HtmlRenderer renderer, TocState options)
    {
        if (Count < 1) return;

        renderer.WriteLine("<ul>");

        foreach (var item in Children)
        {
            if (item.Level > options.MaxLevel)
                continue;

            renderer.WriteLine("<li>");

            if (!item.IsLocator)
            {
                renderer.Write($"<a href=\"#{item.Id}\">{item.Content}</a>");
            }

            item.RenderHtmlLint(renderer, options);

            renderer.WriteLine("</li>");
        }
        renderer.WriteLine("</ul>");
    }
}
