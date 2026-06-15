//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Markdown.RenderInterop;

namespace Bookgen.Lib.Markdown.Renderers.SyntaxRenderPlugins;

internal sealed class MermaidRenderPlugin(IRenderInterop renderInterop) : SyntaxRendererPlugin
{
    public override string LanguageMoniker { get; } = "mermaid";

    public override string Render(string code)
    {
        return RendererImgage(renderInterop.RenderMermaid(code));
    }
}
