using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;
using BookGen.Vfs;

namespace BookGen.Commands;

[CommandName("shortcut")]
internal class CreateShortcut : AsyncCommand
{
    private readonly IWritableFileSystem _fileSystem;

    public CreateShortcut(IWritableFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }


    public override async Task<int> ExecuteAsync(IReadOnlyList<string> context)
    {
        string profileName = TerminalProfileInstaller.GetProfileTitle();

        string shortCutText =
            $"""
            @echo off
            title "Opening Windows terminal with bookgen shell"
            wt -p "{profileName}" --startingDirectory .
            exit
            """;

        await _fileSystem.WriteAllTextAsync("open terminal.cmd", shortCutText);

        return ExitCodes.Success;
    }
}
