using BookGen.Cli;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

internal class NewBookCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly ILogger _logger;
    private readonly IWritableFileSystem _fileSystem;

    public NewBookCommand(ILogger logger, IWritableFileSystem writableFileSystem)
    {
        _logger = logger;
        _fileSystem = writableFileSystem;
    }

    public override async Task<int> Execute(BookGenArgumentBase arguments, string[] context)
    {
        return ExitCodes.Succes;
    }
}
