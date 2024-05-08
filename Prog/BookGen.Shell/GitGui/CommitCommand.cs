//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;

using Spectre.Console;

namespace BookGen.Shell.GitGui;

internal class CommitCommand : GuiCommand
{
    private readonly IAnsiConsole _console;
    private readonly Predicate<string> _canCommit;
    private readonly bool _autoPush;

    public CommitCommand(IAnsiConsole console, Predicate<string> canCommit, bool autoPush)
    {
        _console = console;
        _canCommit = canCommit;
        _autoPush = autoPush;
    }

    public override string DisplayName
        => _autoPush ? "Commit & Push" : "Commit";

    public override int Execute(string workDir, IProgress<string> progress)
    {
        if (!_canCommit.Invoke(workDir))
        {
            _console.MarkupLine("[yellow]No files to commit![/]");
            return GitGuiCodes.ErrorNoCommit;
        }

        var message = _console.Ask<string>("Commit message:");
        if (string.IsNullOrWhiteSpace(message))
        {
            _console.MarkupLine("[red]No commit message provided![/]");
            return GitGuiCodes.ErrorNoCommitMessage;
        }

        var result = ProcessRunner.RunProcess("git", ["commit", "-m", message], workDir, progress);
        if (result == 0 && _autoPush)
        {
            return ProcessRunner.RunProcess("git", ["push"], workDir, progress);
        }
        return result;
    }
}
