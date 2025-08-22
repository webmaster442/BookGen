using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Markdown;

public static class MarkdownProcesor
{
    public static IEnumerable<string> GetImages(string markdown)
    {
        return ProcessLink(markdown, link => link.IsImage);
    }

    public static IEnumerable<string> GetLinks(string markdown)
    {
        return ProcessLink(markdown, link => !link.IsImage);
    }

    private static IEnumerable<string> ProcessLink(string markdown, Predicate<LinkInline> selector)
    {
        MarkdownDocument document = Markdig.Markdown.Parse(markdown);
        foreach (MarkdownObject node in document.Descendants())
        {
            if (node is LinkInline link 
                && selector(link) 
                && !string.IsNullOrEmpty(link.Url))
            {
                yield return link.Url;
            }
        }
    }
}
