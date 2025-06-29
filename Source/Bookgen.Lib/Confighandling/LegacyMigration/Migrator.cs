using System.Diagnostics;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Confighandling.Migration;

public static class Migrator
{
    public static async Task<bool> Migrate(IWritableFileSystem folder, ILogger logger)
    {
        MigrationState state = new();
        IMigrationStep[] steps = 
        [
            new LoadLegacyConfig(),
            new LoadLegacyTags(),
            new LoadLegacyToc(),
            new MigrateToc(),
            new MigrateConfig(),
            new MigrateFiles(),
        ];

        try
        {
            foreach (var step in steps)
            {
                bool result = await step.ExecuteAsync(folder, state, logger);
                if (!result)
                {
                    logger.LogError("Migration step failed: {Step}", step.GetType().Name);
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Migration failed");
            logger.LogDebug("Message: {msg} Stack Trace: {StackTrace}", ex.Message, ex.StackTrace);
            return false;
        }
    }
}
