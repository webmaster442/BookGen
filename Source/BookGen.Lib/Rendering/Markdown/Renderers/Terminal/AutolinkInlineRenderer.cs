//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Web;

using Markdig.Syntax.Inlines;

namespace BookGen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class AutolinkInlineRenderer : TerminalObjectRenderer<AutolinkInline>
{
    protected override void Write(TerminalRenderer renderer, AutolinkInline obj)
    {
        string url = obj.IsEmail
            ? HttpUtility.UrlEncode($"mailto:{obj.Url}")
            : HttpUtility.UrlEncode(obj.Url);

        var text = renderer
            .Builder
            .New()
            .WithForegroundColor(renderer.RenderOptions.LinkColor)
            .AppendLink(url, obj.Url)
            .ResetFormat()
            .ToString();

        renderer.Write(text);
    }
}
