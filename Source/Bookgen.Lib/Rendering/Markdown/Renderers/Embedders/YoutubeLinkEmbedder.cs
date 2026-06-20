//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Embedders;

internal sealed class YoutubeLinkEmbedder : BaseLinkEmbedder
{
    public override bool TryRender(Uri input, [NotNullWhen(true)] out string? rendered)
    {
        if (!IsHost(input, "youtube.com", "youtu.be"))
        {
            rendered = null;
            return false;
        }

        if (!TryGetQueryParam(input, "v", out var videoid))
        {
            rendered = null;
            return false;
        }

        var imgUrl = new Uri($"https://img.youtube.com/vi/{videoid}/sddefault.jpg");

        if (TryDownloadAndBase64Encode(imgUrl, out string? encoded))
        {
            rendered = $"""
                <a href="{input}" target="_blank">
                <img src="data:{MediaTypeNames.Image.Jpeg};base64,{encoded}"><br>
                {input}
                </a>
                """;

            return true;
        }

        rendered = null;
        return false;
    }
}
