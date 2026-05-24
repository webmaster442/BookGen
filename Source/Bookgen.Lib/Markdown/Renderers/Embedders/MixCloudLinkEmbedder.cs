//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace Bookgen.Lib.Markdown.Renderers.Embedders;

internal sealed class MixCloudLinkEmbedder : BaseLinkEmbedder
{
    public override bool TryRender(Uri input, [NotNullWhen(true)] out string? rendered)
    {
        if (!IsHost(input, "mixcloud.com"))
        {
            rendered = null;
            return false;
        }

        string show = HttpUtility.UrlEncode(input.AbsolutePath);

        rendered = $"""
            <iframe width="100%" height="120" src="https://player-widget.mixcloud.com/widget/iframe/?hide_cover=1&feed={show}" frameborder="0" allow="encrypted-media; fullscreen; autoplay; idle-detection; speaker-selection; web-share;" ></iframe>
            """;

        return true;
    }
}
