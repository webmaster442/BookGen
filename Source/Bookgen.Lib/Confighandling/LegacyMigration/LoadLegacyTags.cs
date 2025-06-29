using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Confighandling.LegacyMigration;

internal sealed class LoadLegacyTags : IMigrationStep
{
    public async Task<bool> ExecuteAsync(IWritableFileSystem foler, MigrationState state, ILogger logger)
    {
        var file = Path.Combine(MigrationState.LegacyConfigFolder, MigrationState.LegacyTagsFileName);
        if (!foler.FileExists(file))
        {
            logger.LogWarning("Legacy tags file not found");
            return false;
        }

        var tags = await foler.DeserializeAsync<Dictionary<string, string[]>>(file);

        if (tags == null)
        {
            logger.LogError("Failed to load legacy tags file");
            return false;
        }

        state.LegacyTags = tags;
        return true;
    }
}
