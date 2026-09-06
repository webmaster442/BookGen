//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Spectre.Console;

namespace BookGen.Shellprog;

internal class GitArguments : ArgumentsBase
{
    [Description("Working directory for prompt")]
    [Argument(0, IsOptional = true)]
    public string WorkDirectory { get; set; }

    public GitArguments()
    {
        WorkDirectory = string.Empty;
    }
}

internal abstract class GitCommandBase<T> : Command<T> where T : GitArguments
{
    protected const int TimeOut = 10;

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

            (int exitcode, string? result, string? error) = ProcessRunner.RunProcess("git", arguments, TimeOut, workDir);

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
        (int exitcode, string? output, string? error) = ProcessRunner.RunProcess("git", gitArguments, TimeOut, workDirectory);
        return exitcode == 0 && string.IsNullOrEmpty(error) ? output : string.Empty;
    }

    protected static GitStatus? GetGitStatus(string workDirectory)
    {
        try
        {
            string[] gitArguments = ["status", "-b", "-s", "--porcelain=2"];

            (int exitcode, string? output, string _) = ProcessRunner.RunProcess("git", gitArguments, TimeOut, workDirectory);
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
}
