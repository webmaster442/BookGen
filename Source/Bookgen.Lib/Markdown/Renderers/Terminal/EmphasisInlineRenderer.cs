using Markdig.Syntax.Inlines;

using System.Diagnostics;

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

internal sealed class EmphasisInlineRenderer : TerminalObjectRenderer<EmphasisInline>
{
    private enum RenderAs
    {
        Regular = 0,
        Bold,
        Italic,
    }

    private static RenderAs GetRenderOption(EmphasisInline obj)
    {
        if (obj.DelimiterChar is '*' or '_')
        {
            Debug.Assert(obj.DelimiterCount <= 2);
            return obj.DelimiterCount == 2 ? RenderAs.Bold : RenderAs.Italic;
        }
        return RenderAs.Regular;
    }

    protected override void Write(TerminalRenderer renderer, EmphasisInline obj)
    {
        RenderAs option = GetRenderOption(obj);
        if (option == RenderAs.Regular)
        {
            renderer.WriteChildren(obj);
            return;
        }

        var preformat = renderer.Builder.New();

        if (option == RenderAs.Bold)
            preformat.WithBold();
        else
            preformat.WithItalic();

        renderer.Write(preformat.ToString()).WriteChildren(obj);
        renderer.WriteReset();
    }
}
