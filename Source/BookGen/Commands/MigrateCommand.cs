using Bookgen.Lib.Confighandling.LegacyMigration;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("migrate")]
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

        return value ? ExitCodes.Succes : ExitCodes.GeneralError;
    }
}
