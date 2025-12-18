//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.ImageService;
using Bookgen.Lib.Markdown.TableOfContents;

using Markdig;

namespace Bookgen.Lib.Markdown;

public sealed class MarkdownConverter : IDisposable
{
    private readonly MarkdownPipeline _pipeline;

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

        _pipeline = configuration.Build();
    }

    public void Dispose()
    {
        foreach (IMarkdownExtension? extension in _pipeline.Extensions)
        {
            if (extension is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    public string RenderMarkdownToHtml(string markdown)
        => Markdig.Markdown.ToHtml(markdown, _pipeline);
}
