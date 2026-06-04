//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Markdown.RenderInterop;

namespace Bookgen.Lib.Markdown.Renderers.SyntaxRenderPlugins;

internal sealed class MermaidRenderPlugin : SyntaxRendererPlugin
{
    private readonly IRenderInterop _renderInterop;

    public MermaidRenderPlugin(IRenderInterop renderInterop)
    {
        _renderInterop = renderInterop;
    }
    public override string LanguageMoniker { get; } = "mermaid";

    public override string Render(string code)
    {
        return RendererImgage(_renderInterop.RenderMermaid(code));
    }
}
