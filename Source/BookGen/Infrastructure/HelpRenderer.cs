//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Markdown.Renderers.Terminal;

using Markdig;
using Markdig.Parsers;

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
        string md = string.Join(Environment.NewLine, article);
        var document = MarkdownParser.Parse(md, _terminalPipeLine);

        using var writer = new StringWriter();
        var renderer = new TerminalRenderer(writer, new RenderOptions());

        renderer.Render(document);
        renderer.Writer.Flush();

        using var reader = new StringReader(writer.ToString());

        Webmaster442.WindowsTerminal.Wigets.Pager pager = new(reader);

        pager.Show(false);
    }
}
