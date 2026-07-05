//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using Bookgen.Lib.Confighandling.LegacyMigration;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("migrate")]
[Description("Migrate an old Bookgen book to the new format.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
[ExitCode(ExitCodes.GeneralError, "An error occurred during the conversion.")]
internal sealed class MigrateCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;
    private readonly ILogger _logger;

    public MigrateCommand(IWritableFileSystem writableFileSystem, ILogger logger)
    {
        _writableFileSystem = writableFileSystem;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        _writableFileSystem.Scope = arguments.Directory;

        bool value = await Migrator.Migrate(_writableFileSystem, _logger);

        return value ? ExitCodes.Success : ExitCodes.GeneralError;
    }
}
