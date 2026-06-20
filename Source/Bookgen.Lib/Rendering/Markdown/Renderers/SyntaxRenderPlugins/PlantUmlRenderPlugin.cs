//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Rendering.Markdown.RenderInterop;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.SyntaxRenderPlugins;

internal sealed class PlantUmlRenderPlugin(IRenderInterop renderInterop) : SyntaxRendererPlugin
{
    public override string LanguageMoniker => "plantuml";

    public override string Render(string code)
    {
        return RendererImgage(renderInterop.RenderPlantUml(code));
    }
}
