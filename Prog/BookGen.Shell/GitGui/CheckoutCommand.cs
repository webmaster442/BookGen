//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;

using Spectre.Console;

namespace BookGen.Shell.GitGui;

internal sealed partial class CheckoutCommand : GuiCommand
{
    private readonly IAnsiConsole _console;

    public CheckoutCommand(IAnsiConsole console)
    {
        _console = console;
    }

    public override string DisplayName => "Checkout a brach";

    public override int Execute(string workDir, IProgress<string> progress)
    {
        var (exitcode, output) = ProcessRunner.RunProcess("git", ["branch", "-a"], 10, workDir);
        if (exitcode != 0)
        {
            _console.MarkupLine($"[red]Error: {output.EscapeMarkup()}[/]");
            return exitcode;
        }

        var branchList = GitParser.ParseBranches(output);
        var branch = _console.Prompt(new SelectionPrompt<string>()
            .Title("Select a branch to checkout")
            .PageSize(10)
            .AddChoices(branchList));

        return ProcessRunner.RunProcess("git", ["checkout", branch], workDir, progress);
    }
}
