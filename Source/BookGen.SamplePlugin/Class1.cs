using BookGen.Api;
using BookGen.Api.V1;

namespace BookGen.SamplePlugin;

public sealed class SamplePlugin : IBuildPluginV1
{
    public async Task<bool> Build(IBook book, IBookgenServices bookgenServices, CancellationToken cancellationToken)
    {
        foreach (IChapter chapter in book.Chapters)
        {
        }

        return true;
    }
}
