
using System.Diagnostics.CodeAnalysis;

using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Markdown.Renderers;

internal sealed class ExtendedLinkInlineRenderer : LinkInlineRenderer
{
    protected override void Write(HtmlRenderer renderer, LinkInline obj)
    {
        if (IsInlineSvg(obj))
        {
            renderer.Write(obj.Url);
            return;
        }
        if (IsYoutubeLink(obj))
        {
            EmbedYoutube(renderer, obj);
            return;
        }

        base.Write(renderer, obj);

    }

    private void EmbedYoutube(HtmlRenderer renderer, LinkInline obj)
    {
        if (obj.Url == null)
            throw new InvalidOperationException("YouTube link URL cannot be null.");

        var url = new Uri(obj.Url);

        if (!TryGetQueryParam(url, "v", out var videoid))
            throw new InvalidOperationException("YouTube link does not contain a valid video ID.");

        var code = $"""<iframe width="560" height="315" src="https://www.youtube.com/embed/{videoid}" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>""";
        renderer.Write(code);
    }

    private static bool IsYoutubeLink(LinkInline obj)
        => !obj.IsImage && obj.Url?.Contains("youtube.com/watch?v=") == true;

    private static bool IsInlineSvg(LinkInline obj)
        => obj.IsImage && obj.Url?.Contains("<svg") == true && obj.Url?.Contains("</svg>") == true;

    private static bool TryGetQueryParam(Uri uri, string queryparam, [NotNullWhen(true)] out string? value)
    {
        var query = uri.Query;
        if (string.IsNullOrEmpty(query))
        {
            value = null;
            return false;
        }
        var queryParams = System.Web.HttpUtility.ParseQueryString(query);

        value = queryParams[queryparam];
        return value != null;
    }
}