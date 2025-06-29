using System.Text;

using Bookgen.Lib.Domain.IO;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Confighandling.LegacyMigration;

internal class MigrateFiles : IMigrationStep
{
    public async Task<bool> ExecuteAsync(IWritableFileSystem foler, MigrationState state, ILogger logger)
    {
        logger.LogInformation("Migrating files to add front matter...");
        var serializer = YamlSerializerFactory.CreateSerializer();

        foreach (var chapter in state.LegacyToc.Chapters)
        {
            foreach (var link in state.LegacyToc.GetLinksForChapter(chapter))
            {
                logger.LogDebug("Migrating file: {file}", link.Url);

                string[] tags;
                if (state.LegacyTags.TryGetValue(link.Url, out string[]? value))
                {
                    tags = value;
                }
                else
                {
                    logger.LogWarning("No tags found for file: {file}", link.Url);
                    tags = Array.Empty<string>();
                }

                FrontMatter frontMatter = new FrontMatter
                {
                    Title = link.Text,
                    Tags = string.Join(',', tags),
                };

                const string divider = "---";

                string content = await foler.ReadAllTextAsync(link.Url);

                StringBuilder result = new();
                result.AppendLine(divider)
                      .Append(serializer.Serialize(frontMatter))
                      .AppendLine(divider).AppendLine()
                      .Append(content);

                await foler.WriteAllTextAsync(link.Url, result.ToString());
            }
        }

        return true;
    }
}
