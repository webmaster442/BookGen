//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Rendering.Markdown.Renderers.SyntaxRenderPlugins;
using BookGen.Lib.Rendering.Markdown.RenderInterop;

using Markdig.Extensions.Mathematics;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace BookGen.Lib.Rendering.Markdown.Renderers;

internal sealed class SvgMathInlineRenderer : HtmlObjectRenderer<MathInline>
{
    private readonly LatexRenderPlugin _latexRenderPlugin;

    public SvgMathInlineRenderer(IRenderInterop renderInterop)
    {
        _latexRenderPlugin = new LatexRenderPlugin(renderInterop);
    }

    protected override void Write(HtmlRenderer renderer, MathInline obj)
    {
        if (!obj.Content.Text.StartsWith('$'))
        {
            // If the content does not start with a dollar sign, we assume it's not a valid math expression and render it as plain text.
            renderer.Write(obj.Content.Text);
            return;
        }

        renderer.Write("<span").WriteAttributes(obj).Write(">");
        renderer.Write(_latexRenderPlugin.Render(obj.Content.Text, ""));
        renderer.Write("</span>");
    }
}
