//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("validate")]
internal sealed class ValidateCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;
    private readonly ILogger _logger;

    public ValidateCommand(IWritableFileSystem writableFileSystem, ILogger logger)
    {
        _writableFileSystem = writableFileSystem;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        _writableFileSystem.Scope = arguments.Directory;

        using var environment = new BookEnvironment(_writableFileSystem, _writableFileSystem);

        EnvironmentStatus status = await environment.Initialize(arguments.ConfigOverlay);

        if (!status.IsOk)
        {
            foreach (var issue in status)
            {
                _logger.LogError(issue);
            }

            return ExitCodes.ConfigError;
        }

        _logger.LogInformation("{folder} configuration is ok", arguments.Directory);
        return ExitCodes.Success;
    }
}
