//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Syntax.Inlines;

namespace BookGen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class CodeInlineRenderer : TerminalObjectRenderer<CodeInline>
{
    protected override void Write(TerminalRenderer renderer, CodeInline obj)
    {
        var begin = renderer.Builder
            .New()
            .WithForegroundColor(renderer.RenderOptions.CodeInlineColor)
            .WithItalic()
            .ToString();

        renderer.Write(begin);

        renderer.Write(obj.ContentSpan);

        renderer.WriteReset();
    }
}
