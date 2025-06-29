using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Confighandling.LegacyMigration;

internal interface IMigrationStep
{
    Task<bool> ExecuteAsync(IWritableFileSystem foler, MigrationState state, ILogger logger);
}
