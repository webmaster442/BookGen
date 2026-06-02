//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Markdown.Renderers.SyntaxRenderPlugins;
using Bookgen.Lib.Markdown.RenderInterop;

using Markdig.Extensions.Mathematics;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Bookgen.Lib.Markdown.Renderers;

internal class SvgMathBlockRenderer : HtmlObjectRenderer<MathBlock>
{
    private readonly LatexRenderPlugin _latexRenderPlugin;

    public SvgMathBlockRenderer(IRenderInterop renderInterop)
    {
        _latexRenderPlugin = new LatexRenderPlugin(renderInterop);
    }

    protected override void Write(HtmlRenderer renderer, MathBlock obj)
    {
        renderer.EnsureLine();
        renderer.Write("<div").WriteAttributes(obj).WriteLine(">");
        string code = obj.GetCode();
        renderer.Write(_latexRenderPlugin.Render(code));
        renderer.WriteLine("</div>");
    }
}
