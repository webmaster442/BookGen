using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace BookGen.Lib.Rendering.Markdown.Keyboard;

internal sealed class KeyboardHtmlRenderer : HtmlObjectRenderer<KeyboardInline>
{
    protected override void Write(HtmlRenderer renderer, KeyboardInline obj)
    {
        if (renderer.EnableHtmlForInline)
        {
            renderer.Write("<kbd>");
            renderer.Write(obj.Text);
            renderer.Write("</kbd>");
        }
        else
        {
            renderer.Write(obj.Text);
        }
    }
}
