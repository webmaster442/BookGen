//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Syntax;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class HeadingRenderer : TerminalObjectRenderer<HeadingBlock>
{
    protected override void Write(TerminalRenderer renderer, HeadingBlock obj)
    {
        string prefix = new string('#', obj.Level);

        var beginText = renderer
            .Builder
            .New()
            .WithForegroundColor(renderer.RenderOptions.HeadingColor)
            .WithBold()
            .Append(prefix)
            .Append(' ')
            .ToString();

        renderer
            .Write(beginText)
            .WriteLeafInline(obj)
            .WriteReset()
            .EnsureLine()
            .WriteLine();
    }
}
