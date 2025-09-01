using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;
using BookGen.Vfs;

namespace BookGen.Commands;

[CommandName("shortcut")]
internal class ShortcutCommand : AsyncCommand
{
    private readonly IWritableFileSystem _fileSystem;

    public ShortcutCommand(IWritableFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public override SupportedOs SupportedOs => SupportedOs.Windows;

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
