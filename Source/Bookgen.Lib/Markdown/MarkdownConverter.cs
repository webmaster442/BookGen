//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Markdown.Renderers.Terminal;
using Bookgen.Lib.Markdown.TableOfContents;
using Bookgen.Lib.Pipeline;

using Markdig;

using Microsoft.AspNetCore.Components;

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

    public string RenderMarkdownToTerminal(string markdown)
    {
        PSMarkdownOptionInfo optionInfo = new();

        using var writer = new StringWriter();
        var renderer = new VT100Renderer(writer, optionInfo);

        return Markdig.Markdown.Convert(markdown, renderer, _terminalPipeLine).ToString() ?? "";
    }
}
