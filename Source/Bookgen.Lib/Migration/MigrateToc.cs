using Bookgen.Lib.Domain.IO;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Migration;

internal sealed class MigrateToc : IMigrationStep
{
    public async Task<bool> ExecuteAsync(IWritableFileSystem foler, MigrationState state, ILogger logger)
    {
        logger.LogInformation("Migrating table of contents...");
        List<TocChapter> chapters = new List<TocChapter>();
        foreach (var chapter in state.LegacyToc.Chapters)
        {
            TocChapter migratedChapter = new()
            {
                Title = chapter,
                SubTitle = string.Empty,
                Files = state.LegacyToc.GetLinksForChapter(chapter).Select(l => l.Url).ToArray()
            };
            chapters.Add(migratedChapter);
        }

        TableOfContents tableOfContents = new TableOfContents()
        {
            Chapters = chapters.ToArray()
        };

        await foler.SerializeAsync(FileNameConstants.TableOfContents, tableOfContents);

        return true;
    }
}
