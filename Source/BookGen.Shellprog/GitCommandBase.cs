//-----------------------------------------------------------------------------
// (c) 2024-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Spectre.Console;

namespace BookGen.Shellprog;

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

    public enum GitDirectoryStatus
    {
        NotGitDirectory,
        GitDirectory,
        UntrustedGitDirectory
    }

    protected static GitDirectoryStatus TestIfGitDir(string workDir)
    {
        try
        {
            string[] arguments = ["rev-parse", "--is-inside-work-tree"];

            var (exitcode, result, error) = ProcessRunner.RunProcess("git", arguments, TimeOut, workDir);

            if (exitcode == 128 && error.Contains("detected dubious ownership"))
            {
                return GitDirectoryStatus.UntrustedGitDirectory;
            }
            else if (exitcode == 0)
            {
                return bool.TryParse(result, out bool parsed) && parsed ? GitDirectoryStatus.GitDirectory : GitDirectoryStatus.NotGitDirectory;
            }

            return GitDirectoryStatus.NotGitDirectory;
        }
        catch (Exception)
        {
            return GitDirectoryStatus.NotGitDirectory;
        }
    }

    protected static string GetGitRemote(string workDirectory)
    {
        string[] gitArguments = ["config", "--get", "remote.origin.url"];
        var (exitcode, output, error) = ProcessRunner.RunProcess("git", gitArguments, TimeOut, workDirectory);
        return exitcode == 0 && string.IsNullOrEmpty(error) ? output : string.Empty;
    }

    protected static GitStatus? GetGitStatus(string workDirectory)
    {
        try
        {
            string[] gitArguments = ["status", "-b", "-s", "--porcelain=2"];

            var (exitcode, output, _) = ProcessRunner.RunProcess("git", gitArguments, TimeOut, workDirectory);
            if (exitcode == 0)
            {
                return GitParser.ParseStatus(output);
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected void PrintUntrusted()
    {
        var builder = new TerminalOutputBuilder()
            .Append(TerminalOutputBuilder.ForegroundColor.Yellow, TerminalOutputBuilder.BackgroundColor.Black, "<untrusted>");

        _console.WriteLine(builder.ToString());
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
