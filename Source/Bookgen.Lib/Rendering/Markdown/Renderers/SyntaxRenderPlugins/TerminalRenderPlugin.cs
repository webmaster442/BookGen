//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Web;

using Bookgen.Lib.Rendering.Markdown.Renderers;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.SyntaxRenderPlugins;

internal sealed class TerminalRenderPlugin : SyntaxRendererPlugin
{
    private const string TerminalHtml = """
        <div style="margin-bottom: 1rem;" class="terminaloutput">
        <div style="background-color: #877EC2; color: #eee8d6; padding: 3px;">$</div>
        <pre style="text-align: left; font-size: 1.1em; line-height: 1.5; margin: 0px; padding: 8px; background-color: #282c34; color: #DCDFE4; font-family: Monaco, Menlo, Consolas, 'Courier New', monospace; word-break: break-all; word-wrap: break-word; overflow: auto;"><code style="tab-size: 4;"><!--{Code}--></code></pre>
        </div>
        """;

    public override string LanguageMoniker { get; } = "terminal";

    public override string Render(string code)
    {
        const string codeTag = "<!--{Code}-->";
        return TerminalHtml.Replace(codeTag, TerminalRenderer.RenderAnsiCode(HttpUtility.HtmlEncode(code)));
    }
}
