//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Rendering.Markdown;

using BookGen.Lib.Markdown;
using BookGen.Lib.Rendering.Markdown.Renderers.Terminal;

using Markdig;
using Markdig.Parsers;
using Markdig.Syntax;

namespace BookGen.Lib.Rendering.Markdown;

public sealed class MarkdownConverter : IDisposable
{
    private readonly MarkdownPipeline _htmlPipeLine;
    public MarkdownConverter(MarkdownRenderSettings settings)
    {
        MarkdownPipelineBuilder configuration = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseTableOfContents()
            .UseMathematics()
            .UseAlertBlocks()
            .UseKeyboard()
            .UseYamlFrontMatter()
            .Use<BookGenExtension>();

        foreach (IMarkdownExtension extension in configuration.Extensions)
        {
            if (extension is BookGenExtension bookGenExtension)
            {
                bookGenExtension.Inject(settings);
            }
        }

        _htmlPipeLine = configuration.Build();
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

    public string RenderToPlainText(string markdown)
        => Markdig.Markdown.ToPlainText(markdown, _htmlPipeLine);

    public static string RenderMarkdownToTerminal(string markdown, RenderOptions? renderOptions = null)
    {
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
            .UseAutoLinks()
            .UseYamlFrontMatter()
            .Build();

        MarkdownDocument document = MarkdownParser.Parse(markdown, pipeline);

        using var writer = new StringWriter();

        renderOptions ??= new RenderOptions();

        TerminalRenderer renderer = new TerminalRenderer(writer, renderOptions);

        renderer.Render(document);
        renderer.Writer.Flush();

        return renderer.Writer.ToString() ?? string.Empty;
    }
}
