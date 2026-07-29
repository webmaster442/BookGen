//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Rendering.Markdown.RenderInterop;

namespace BookGen.Lib.Rendering.Markdown.Renderers.SyntaxRenderPlugins;

internal sealed class NomnomlRenderPlugin(IRenderInterop renderInterop) : SyntaxRendererPlugin
{
    public override string[] LanguageMonikers { get; } = ["nomnoml"];

    public override string Render(string code, string parsedLanguageMoniker)
    {
        return RendererImgage(renderInterop.RenderNomnoml(code));
    }
}
