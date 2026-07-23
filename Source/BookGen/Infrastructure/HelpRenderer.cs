//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Infrastructure.Terminal;
using BookGen.Lib.Rendering.Markdown;

namespace BookGen.Infrastructure;

internal static class HelpRenderer
{
    public static void RenderHelp(string markdown)
    {
        string rendered = MarkdownConverter.RenderMarkdownToTerminal(markdown);

        Pager pager = new(rendered);

        pager.Show(false);
    }

    public static void RenderHelp(IEnumerable<string> article)
    {
        string md = string.Join(Environment.NewLine, article);
        RenderHelp(md);
    }
}

