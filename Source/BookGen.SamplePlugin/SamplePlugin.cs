using System.Text;

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

        await AddToContentHtml(contentHtml, book.Index, renderer);

        foreach (IChapter chapter in book.Chapters)
        {
            foreach (IDocument document in chapter.Documents)
            {
                await AddToContentHtml(contentHtml, document, renderer);
            }
        }

        return true;
    }

    private async Task AddToContentHtml(HtmlBuilder contentDivs, IDocument document, IRenderer renderer)
    {
        (string content, _) = await document.ReadContent();

        contentDivs.Element("div",
                            tag => tag.Id(GetId(document.FilePath).ToString()),
                            div => div.Raw(renderer.RenderMarkdownToRawHtml(content)));
    }

    private static uint GetId(string filePath)
    {
        uint result = 2166136261;
        foreach (char c in filePath)
        {
            result ^= c;
            result *= 16777619;
        }
        return result;
    }
}
