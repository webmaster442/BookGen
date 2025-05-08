//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;

namespace BookGen.Shell.GitAutocomplete;

internal static class GitCommandProvider
{
    private static IEnumerable<string> GetBranches(string folder)
    {
        var (exitcode, output) = ProcessRunner.RunProcess("git", ["branch", "-a"], 10, folder);

        if (exitcode != 0)
            return Enumerable.Empty<string>();

        return GitParser.ParseBranches(output).Order();
    }

    public static IEnumerable<string> GetGitCommands(string folder)
    {
        var branches = GetBranches(folder);

        yield return "git add .";
        yield return "git add";
        yield return "git branch";
        foreach (var branch in branches)
        {
            yield return $"git checkout {branch}";
        }
        yield return "git clean -fdx";
        yield return "git clean";
        yield return "git clone";
        yield return "git commit -m ";
        yield return "git commit";
        yield return "git fetch";
        yield return "git gc";
        yield return "git init";
        yield return "git lfs ext list";
        yield return "git lfs install";
        yield return "git lfs track";
        yield return "git lfs untrack";
        yield return "git log";
        foreach (var branch in branches)
        {
            yield return $"git merge {branch}";
        }
        yield return "git pull";
        yield return "git push";
        yield return "git reset --hard";
        yield return "git reset";
        yield return "git status";
        yield return "git tag";
    }
}
