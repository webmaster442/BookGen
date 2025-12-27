//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Markdown.Renderers.Terminal;

using Markdig;

using Microsoft.AspNetCore.Components.RenderTree;

namespace BookGen.Infrastructure;

internal sealed class HelpRenderer
{
    private readonly MarkdownPipeline _terminalPipeLine;

    public HelpRenderer()
    {
        _terminalPipeLine = new MarkdownPipelineBuilder().Build();
    }

    public void RenderHelp(IEnumerable<string> article)
    {
        string markdown = string.Join(Environment.NewLine, article);
        using var writer = new StringWriter();
        var renderer = new VT100Renderer(writer, new PSMarkdownOptionInfo());
        string rendered =  Markdown.Convert(markdown, renderer, _terminalPipeLine).ToString() ?? "";

        using var reader = new StringReader(rendered);

        Webmaster442.WindowsTerminal.Wigets.Pager pager = new(reader);

        pager.Show(false);
    }
}
