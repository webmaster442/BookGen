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

    protected void PrintStatus(GitStatus? status)
    {
        if (status is null)
        {
            return;
        }

        var builder = new TerminalOutputBuilder()
            .Append(TerminalOutputBuilder.ForegroundColor.Default, TerminalOutputBuilder.BackgroundColor.Green, $"({status.BranchName}) ");

        if (status.IncommingCommits > 0)
        {
            builder.Append(TerminalOutputBuilder.ForegroundColor.Black,
                           TerminalOutputBuilder.BackgroundColor.Magenta,
                           $"↓: {status.IncommingCommits}");
        }
        if (status.OutGoingCommits > 0)
        {
           builder.Append(TerminalOutputBuilder.ForegroundColor.Black,
                          TerminalOutputBuilder.BackgroundColor.Yellow,
                          $" ↑: {status.OutGoingCommits}");
        }
        if (status.NotCommitedChanges > 0)
        {
            builder.Append(TerminalOutputBuilder.ForegroundColor.Black,
                           TerminalOutputBuilder.BackgroundColor.Cyan,
                           $" M: {status.NotCommitedChanges}");
        }

        _console.WriteLine(builder.ToString());
    }
}
