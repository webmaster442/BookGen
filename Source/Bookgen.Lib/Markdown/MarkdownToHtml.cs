using Bookgen.Lib.ImageService;
using Bookgen.Lib.Markdown.TableOfContents;

using Markdig;

namespace Bookgen.Lib.Markdown;

public sealed class MarkdownToHtml : IDisposable
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdownToHtml(IImgService imgService, RenderSettings settings)
    {
        var configuration = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseTableOfContents()
            .UseMathematics()
            .Use<BookGenExtension>();

        foreach (var extension in configuration.Extensions)
        {
            if (extension is BookGenExtension bookGenExtension)
            {
                bookGenExtension.Inject(imgService, settings);
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
