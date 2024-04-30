//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Annotations;
using BookGen.Shell.GitGui;

using Spectre.Console;

namespace BookGen.Shell.Commands;

[CommandName("gitgui")]
internal class GitGuiCommand : GitCommandBase, IProgress<string>
{
    private readonly GuiCommand[] _commands;

    private bool Confirm(string message)
        => _console.Confirm(message);

    public GitGuiCommand(IAnsiConsole console) : base(console)
    {
        _commands =
        [
            new RunCommand("fetch", ["fetch", "--no-auto-gc"]),
            new RunCommand("pull", ["pull", "--progress"]),
            new RunCommand("last 20 log entry", ["log", "--graph", "--date=relative", "--format=format:'%C(auto)%h %C(bold blue)%an%C(auto)%d %C(green)%ad%C(reset)%n%w(80,8,8)%s'", "--max-count=20"]),
            new CheckoutCommand(_console),
            new RunCommand("hard reset", ["reset", "--hard"], () => Confirm("Are you sure you want to reset?")),
            new DelegateCommand("exit", () => Environment.Exit(0)),
        ];
    }

    void IProgress<string>.Report(string value)
    {
        _console.WriteLine(value);
    }

    private int CloneMenu(string workDirectory)
    {
        return new CloneCommand(_console)
            .Execute(workDirectory, this);
    }

    private void InteractiveMenu(string workdir)
    {
        var selector = new SelectionPrompt<GuiCommand>()
                       .Title("Select an action")
                       .PageSize(10)
                       .UseConverter(cmd => cmd.DisplayName)
                       .AddChoices(_commands);

        while (true)
        {
            _console.Clear();

            var path = new TextPath(workdir)
            {
                RootStyle = new Style(foreground: Color.Red),
                SeparatorStyle = new Style(foreground: Color.Green),
                StemStyle = new Style(foreground: Color.Blue),
                LeafStyle = new Style(foreground: Color.Yellow)
            };

            _console.Markup("[green]Repo: [/]");
            _console.Write(path);
            _console.WriteLine();
            GetGitStatus(workdir, true);
            AnsiConsole.Write(new Rule());

            var choice = _console.Prompt(selector);
            choice.Execute(workdir, this);

            new TextPrompt<string>("Press any key to continue")
                .AllowEmpty()
                .Show(_console);
        }
    }

    public override int Execute(GitArguments arguments, string[] context)
    {
        if (string.IsNullOrEmpty(arguments.WorkDirectory))
        {
            _console.MarkupLine("[red]Error: Work directory not set![/]");
            return GitGuiCodes.ErrorNoWorkdir;
        }

        if (!TestIfGitDir(arguments.WorkDirectory))
        {
            return CloneMenu(arguments.WorkDirectory);
        }
        else
        {
            InteractiveMenu(arguments.WorkDirectory);
        }

        return 0;
    }
}
