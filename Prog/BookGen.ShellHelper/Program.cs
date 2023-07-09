//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Text;

using BookGen.Domain.Terminal;
using BookGen.DomainServices;
using BookGen.ShellHelper;

[assembly: InternalsVisibleTo("BookGen.Tests")]

Console.OutputEncoding = Encoding.UTF8;

const string promptMode = "prompt";
const int timeOut = 10;

if (args.Length == 2
    && args[0] == promptMode)
{
    DoPrompt(args[1]);
}

static void DoPrompt(string workDir)
{
    if (!string.IsNullOrEmpty(workDir)
        && TestIfGitDir(workDir))
    {
        var arguments = new string[] { "status", "-b", "-s", "--porcelain=2" };

        var (exitcode, output) = ProcessRunner.RunProcess("git", arguments, timeOut);
        if (exitcode == 0)
        {
            var status = GitParser.ParseStatus(output);
            PrintStatus(status);
        }
    }
}

static void PrintStatus(GitStatus status)
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

    Console.WriteLine(text);
}

static bool TestIfGitDir(string workDir)
{
    var arguments = new string[] { "status", "2" };

    var (exitcode, _) = ProcessRunner.RunProcess("git", arguments, timeOut, workDir);
    return exitcode == 0;
}