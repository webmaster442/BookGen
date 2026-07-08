//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.V1;

namespace BookGen.Infrastructure.Plugins.V1;

internal sealed class Chapter : IChapter
{
    public Chapter(string title, List<Document> documents)
    {
        Title = title;
        Documents = documents;
    }

    public string Title { get; }

    public IReadOnlyList<IDocument> Documents { get; }
}
