//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Lib;
using BookGen.Lib.Domain.IO;
using BookGen.Lib.Domain.IO.Configuration;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Book;

[CommandName("newbook")]
[Description("Creates a new book structure in the given folder.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
[ExitCode(ExitCodes.GeneralError, "The specified folder contains an existing book structure.")]
internal sealed class NewBookCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly ILogger _logger;
    private readonly IWritableFileSystem _fileSystem;

    public NewBookCommand(ILogger logger, IWritableFileSystem writableFileSystem)
    {
        _logger = logger;
        _fileSystem = writableFileSystem;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        _fileSystem.Scope = arguments.Directory;

        if (BookEnvironment.IsBookGenFolder(_fileSystem.Scope))
        {
            _logger.LogWarning("{folder} is a bookGen folder. Exiting", _fileSystem.Scope);
            return ExitCodes.GeneralError;
        }

        _logger.LogInformation("Creating {config}...", FileNameConstants.ConfigFile);
        await _fileSystem.SerializeAsync(FileNameConstants.ConfigFile, new Config(), writeSchema: true);

        _logger.LogInformation("Creating {toc}...", FileNameConstants.TableOfContents);
        await _fileSystem.SerializeAsync(FileNameConstants.TableOfContents, new BookGen.Lib.Domain.IO.TableOfContents(), writeSchema: true);

        return ExitCodes.Success;
    }
}
