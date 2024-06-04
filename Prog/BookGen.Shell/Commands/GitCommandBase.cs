//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Domain.Terminal;
using BookGen.DomainServices;
using BookGen.ShellHelper;

using Spectre.Console;

namespace BookGen.Shell.Commands;
internal abstract class GitCommandBase : Command<GitCommandBase.GitArguments>
{
    protected const int TimeOut = 10;
    protected readonly IAnsiConsole _console;

    internal sealed class GitArguments : ArgumentsBase
    {
        [Argument(0, IsOptional = true)]
        public string WorkDirectory { get; set; }
        public GitArguments()
        {
            WorkDirectory = string.Empty;
        }
    }

    protected GitCommandBase(IAnsiConsole console)
    {
        _console = console;
    }

    protected bool TestIfGitDir(string workDir)
    {
        var arguments = new string[] { "status", "2" };

        var (exitcode, _) = ProcessRunner.RunProcess("git", arguments, TimeOut, workDir);
        return exitcode == 0;
    }

    protected GitStatus? GetGitStatus(string workDirectory)
    {
        var gitArguments = new string[] { "status", "-b", "-s", "--porcelain=2" };

        var (exitcode, output) = ProcessRunner.RunProcess("git", gitArguments, TimeOut, workDirectory);
        if (exitcode == 0)
        {
            var status = GitParser.ParseStatus(output);

            return status;
        }

        return null;
    }

    protected void PrintLongStatus(GitStatus? status)
    {
        if (status is null)
        {
            return;
        }
        _console.MarkupLine($"[green]Branch:            {status.BranchName.EscapeMarkup()}[/]");
        _console.MarkupLine($"[magenta]Incomming commits: {status.IncommingCommits}[/]");
        _console.MarkupLine($"[yellow]Outgoing commits:  {status.OutGoingCommits}[/]");
        _console.MarkupLine($"[cyan]Modified:          {status.NotCommitedChanges}[/]");
    }

    protected void PrintStatus(GitStatus? status)
    {
        if (status is null)
        {
            return;
        }
        string text = new TerminalStringBuilder()
            .BackgroundGreen()
            .Text($"({status.BranchName}) ")
            .ForegroundBlack()
            .BackgroundMagenta()
            .Text("↓: ")
            .Text(status.IncommingCommits)
            .BackgroundYellow()
            .Text(" ↑: ")
            .Text(status.OutGoingCommits)
            .BackgroundCyan()
            .Text(" M: ")
            .Text(status.NotCommitedChanges)
            .Default()
            .ToString();

        _console.WriteLine(text);
    }
}
