//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Confighandling.LegacyMigration;

internal interface IMigrationStep
{
    Task<bool> ExecuteAsync(IWritableFileSystem foler, MigrationState state, ILogger logger);
}
