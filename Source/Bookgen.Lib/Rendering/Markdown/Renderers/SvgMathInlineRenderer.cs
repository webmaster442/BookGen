//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Rendering.Markdown.Renderers.SyntaxRenderPlugins;
using Bookgen.Lib.Rendering.Markdown.RenderInterop;

using Markdig.Extensions.Mathematics;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Bookgen.Lib.Rendering.Markdown.Renderers;

internal sealed class SvgMathInlineRenderer : HtmlObjectRenderer<MathInline>
{
    private readonly LatexRenderPlugin _latexRenderPlugin;

    public SvgMathInlineRenderer(IRenderInterop renderInterop)
    {
        _latexRenderPlugin = new LatexRenderPlugin(renderInterop);
    }

    protected override void Write(HtmlRenderer renderer, MathInline obj)
    {
        renderer.Write("<span").WriteAttributes(obj).Write(">");
        renderer.Write(_latexRenderPlugin.Render(obj.Content.Text));
        renderer.Write("</span>");
    }
}
