using Bookgen.Lib;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("newbook")]
internal sealed class NewBookCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly ILogger _logger;
    private readonly IWritableFileSystem _fileSystem;
    private readonly ProgramInfo _programInfo;

    public NewBookCommand(ILogger logger, IWritableFileSystem writableFileSystem, ProgramInfo programInfo)
    {
        _logger = logger;
        _fileSystem = writableFileSystem;
        _programInfo = programInfo;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        _programInfo.EableVerboseLogging(arguments.Verbose);
        _fileSystem.Scope = arguments.Directory;

        if (BookEnvironment.IsBookGenFolder(_fileSystem.Scope))
        {
            _logger.LogWarning("{folder} is a bookGen folder. Exiting", _fileSystem.Scope);
            return ExitCodes.GeneralError;
        }

        _logger.LogInformation("Creating {config}...", FileNameConstants.ConfigFile);
        await _fileSystem.SerializeAsync(FileNameConstants.ConfigFile, new Config());

        _logger.LogInformation("Creating {toc}...", FileNameConstants.TableOfContents);
        await _fileSystem.SerializeAsync(FileNameConstants.TableOfContents, new TableOfContents());

        return ExitCodes.Succes;
    }
}
