//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Domain.Terminal;
using BookGen.DomainServices;
using BookGen.ShellHelper;

using Spectre.Console;

namespace BookGen.Shell.Commands;

[CommandName("prompt")]
internal sealed class Prompt : Command<Prompt.PromptArguments>
{
    private const int TimeOut = 10;
    private readonly IAnsiConsole _console;

    internal class PromptArguments : ArgumentsBase
    {
        [Argument(0, IsOptional = true)]
        public string WorkDirectory { get; set; }
        public PromptArguments()
        {
            WorkDirectory = string.Empty;
        }
    }

    public Prompt(IAnsiConsole console)
    {
        _console = console;
    }

    private static bool TestIfGitDir(string workDir)
    {
        var arguments = new string[] { "status", "2" };

        var (exitcode, _) = ProcessRunner.RunProcess("git", arguments, TimeOut, workDir);
        return exitcode == 0;
    }

    private void PrintStatus(GitStatus status)
    {
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

    public override int Execute(PromptArguments arguments, string[] context)
    {
        if (!string.IsNullOrEmpty(arguments.WorkDirectory)
            && TestIfGitDir(arguments.WorkDirectory))
        {
            var gitArguments = new string[] { "status", "-b", "-s", "--porcelain=2" };

            var (exitcode, output) = ProcessRunner.RunProcess("git", gitArguments, TimeOut);
            if (exitcode == 0)
            {
                var status = GitParser.ParseStatus(output);
                PrintStatus(status);
            }
        }
        return 0;
    }
}
