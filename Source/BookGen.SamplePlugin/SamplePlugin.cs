using BookGen.Api;
using BookGen.Api.V1;

namespace BookGen.SamplePlugin;

public sealed class SamplePlugin : IBuildPluginV1
{
    public async Task<bool> Build(IBook book, IBookgenServices bookgenServices, CancellationToken cancellationToken)
    {
        HtmlBuilder contentHtml = new(32 * 1024);
        HtmlBuilder navHtml = new(4 * 1024);
        
        IRenderer renderer = bookgenServices.CreateRenderer(new RendererOptions
        {
            SvgRecode = RendererOptions.ImageOption.Passtrough,
        });

        bookgenServices.Logger.LogDebug("Processing document: {DocumentPath}", book.Index.FilePath);
        (string content, IDocumentFrontMatter frontMatter) indexData = await book.Index.ReadContent();

        string IndexHtml = renderer.RenderMarkdownToRawHtml(indexData.content);

        Add(contentHtml, navHtml, indexData, renderer);

        foreach (IChapter chapter in book.Chapters)
        {
            foreach (IDocument document in chapter.Documents)
            {
                bookgenServices.Logger.LogDebug("Processing document: {DocumentPath}", document.FilePath);
                (string content, IDocumentFrontMatter frontMatter) docData = await document.ReadContent();
                Add(contentHtml, navHtml, docData, renderer);
            }
        }

        if (navHtml.LastUnclosedTag != null)
            navHtml.CloseOpenTag();

        RenderTags tags = new()
        {
            Content = contentHtml.ToString(),
            Title = indexData.frontMatter.Title,
            AdditionalData = new()
            {
                { "Navigation", navHtml.ToString() },
                { "Index", IndexHtml }
            }
        };

        string template = Helpers.ReadEmbeddedFile("BookGen.SamplePlugin.Template.html");

        string rendered = renderer.RenderMarkdownToHtml(template, tags);

        await bookgenServices.OutputFolder.WriteTextFile("rendered.html", rendered);

        return true;
    }

    private void Add(HtmlBuilder contentHtml,
                     HtmlBuilder navHtml,
                     (string content, IDocumentFrontMatter frontMatter) docData,
                     IRenderer renderer)
    {
        contentHtml.Element("div",
                    tag => tag.Id(Helpers.GetId(docData.frontMatter.Title).ToString()),
                    div => div.Raw(renderer.RenderMarkdownToRawHtml(docData.content)));

        if (navHtml.LastUnclosedTag == "ul")
        {
            navHtml.Element("li",
                li => li.Element("a", tag => tag.Attr("href", "#"),
                a => a.Text(docData.frontMatter.Title)));
        }
        else
        {
            navHtml.Element("ul");
        }
    }
}
