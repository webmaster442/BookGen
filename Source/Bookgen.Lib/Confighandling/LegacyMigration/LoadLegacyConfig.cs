using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Confighandling.LegacyMigration;

internal sealed class LoadLegacyConfig : IMigrationStep
{
    public async Task<bool> ExecuteAsync(IWritableFileSystem foler, MigrationState state, ILogger logger)
    {
        var file = Path.Combine(MigrationState.LegacyConfigFolder, MigrationState.LegacyConfigFileName);

        if (!foler.FileExists(file))
        {
            logger.LogWarning("Legacy config file not found");
            return false;
        }

        var config = await foler.DeserializeAsync<Domain.IO.Legacy.Config>(file);
        if (config == null)
        {
            logger.LogError("Failed to load legacy config file");
            return false;
        }

        const int minVersion = 1014;

        if (config.Version < minVersion)
        {
            logger.LogError("Legacy config file version {Version} is not supported. Config needs to be at least version: {minVersion}", config.Version, minVersion);
            return false;
        }

        state.LegacyConfig = config;

        return true;
    }
}
