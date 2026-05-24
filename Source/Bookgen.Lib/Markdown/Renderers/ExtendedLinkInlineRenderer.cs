//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Markdown.Renderers.Embedders;

using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Markdown.Renderers;

internal sealed class ExtendedLinkInlineRenderer : LinkInlineRenderer
{
    private readonly bool _autoEmbedSupportedLinks;
    private readonly BaseLinkEmbedder[] _embedders;

    public ExtendedLinkInlineRenderer(bool autoEmbedSupportedLinks)
    {
        _autoEmbedSupportedLinks = autoEmbedSupportedLinks;
        _embedders =
        [
            new YoutubeLinkEmbedder(),
            new MixCloudLinkEmbedder(),
        ];
    }

    protected override void Write(HtmlRenderer renderer, LinkInline obj)
    {
        if (IsInlineSvg(obj))
        {
            renderer.Write(obj.Url);
            return;
        }

        if (_autoEmbedSupportedLinks && !string.IsNullOrEmpty(obj.Url))
        {
            Uri uri = new Uri(obj.Url);
            foreach (var embedder in _embedders)
            {
                if (embedder.TryRender(uri, out string? rendered))
                {
                    renderer.Write(rendered);
                    return;
                }
            }
        }

        base.Write(renderer, obj);

    }

    private static bool IsInlineSvg(LinkInline obj)
        => obj.IsImage && obj.Url?.Contains("<svg") == true && obj.Url?.Contains("</svg>") == true;
}
