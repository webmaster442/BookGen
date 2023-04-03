//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

using BookGen.DomainServices;

[assembly: InternalsVisibleTo("BookGen.Tests")]

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
            Console.WriteLine("({0}) ↓: {1} ↑: {2} M: {3}",
                              status.BranchName,
                              status.IncommingCommits,
                              status.OutGoingCommits,
                              status.NotCommitedChanges);
        }
    }
}

static bool TestIfGitDir(string workDir)
{
    var arguments = new string[] { "status", "2" };

    var (exitcode, _) = ProcessRunner.RunProcess("git", arguments, timeOut, workDir);
    return exitcode == 0;
}