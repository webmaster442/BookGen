//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib;
using Bookgen.Lib.Domain.IO;

using BookGen.Api.V1;

using Microsoft.Extensions.Logging;

namespace BookGen.PluginInfrastructure.V1;

internal sealed class Book : IBook
{
    public IDocument Index { get; }
    public IReadOnlyList<IChapter> Chapters { get; }

    private Book(IDocument index, List<IChapter> chapters)
    {
        Index = index;
        Chapters = chapters;
    }

    public static Book CreateFrom(BookEnvironment environment, ILogger logger)
    {
        var chapters = new List<IChapter>();
        foreach (TocChapter chapter in environment.TableOfContents.Chapters)
        {
            var documents = new List<Document>();
            foreach (string fileName in chapter.Files)
            {
                var doc = new Document(environment.Source, logger, fileName);
                documents.Add(doc);
            }
            chapters.Add(new Chapter(chapter.Title, documents));
        }
        return new Book(new Document(environment.Source, logger, environment.TableOfContents.IndexFile), chapters);
    }
}
