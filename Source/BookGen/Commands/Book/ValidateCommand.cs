//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Lib;
using BookGen.Lib.AppSettings;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Book;

[CommandName("validate")]
[Description("Validate the configuration files used by bookgen in the specified folder.")]
[ExitCode(ExitCodes.Success, "The configuration is valid.")]
[ExitCode(ExitCodes.ConfigError, "The configuration was invalid.")]
internal sealed class ValidateCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;
    private readonly ILogger _logger;
    private readonly IProgramPathResolver _programPathResolver;

    public ValidateCommand(IWritableFileSystem writableFileSystem, IProgramPathResolver programPathResolver, ILogger logger)
    {
        _writableFileSystem = writableFileSystem;
        _programPathResolver = programPathResolver;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        _writableFileSystem.Scope = arguments.Directory;

        using var environment = new BookEnvironment(_writableFileSystem, _writableFileSystem, _programPathResolver);

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
