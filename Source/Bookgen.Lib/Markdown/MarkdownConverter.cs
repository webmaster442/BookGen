//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Markdown.Renderers.Terminal;
using Bookgen.Lib.Markdown.TableOfContents;

using Markdig;
using Markdig.Parsers;

namespace Bookgen.Lib.Markdown;

public sealed class MarkdownConverter : IDisposable
{
    private readonly MarkdownPipeline _htmlPipeLine;
    private readonly MarkdownPipeline _terminalPipeLine;

    public MarkdownConverter(RenderSettings settings)
    {
        var configuration = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseTableOfContents()
            .UseMathematics()
            .UseYamlFrontMatter()
            .Use<BookGenExtension>();

        foreach (var extension in configuration.Extensions)
        {
            if (extension is BookGenExtension bookGenExtension)
            {
                bookGenExtension.Inject(settings);
            }
        }

        _htmlPipeLine = configuration.Build();

        _terminalPipeLine = new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .UseAutoLinks()
            .Build();
    }

    public void Dispose()
    {
        foreach (IMarkdownExtension? extension in _htmlPipeLine.Extensions)
        {
            if (extension is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    public string RenderMarkdownToHtml(string markdown)
        => Markdig.Markdown.ToHtml(markdown, _htmlPipeLine);

    public string RenderMarkdownToTerminal(string markdown, RenderOptions? renderOptions = null)
    {
        var document = MarkdownParser.Parse(markdown, _terminalPipeLine);

        using var writer = new StringWriter();

        renderOptions ??= new RenderOptions();

        TerminalRenderer renderer = new TerminalRenderer(writer, renderOptions);

        renderer.Render(document);
        renderer.Writer.Flush();

        return renderer.Writer.ToString() ?? string.Empty;
    }
}
