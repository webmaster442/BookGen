//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Spectre.Console;

namespace BookGen.Shellprog;

[CommandName("repoweb")]
internal sealed class RepoWeb : GitCommandBase
{
    public RepoWeb(IAnsiConsole console) : base(console)
    {
    }

    public override int Execute(GitArguments arguments, IReadOnlyList<string> context)
    {
        if (!string.IsNullOrEmpty(arguments.WorkDirectory)
            && TestIfGitDir(arguments.WorkDirectory) == GitDirectoryStatus.GitDirectory)
        {
            var remote = GetGitRemote(arguments.WorkDirectory);
            var url = GitParser.GetRepoWebUrl(remote);
            if (string.IsNullOrEmpty(url))
            {
                _console.MarkupLine("[red]No suitable remote URL found.[/]");
                return 1;
            }
            _console.MarkupLine($"[green]Opening remote URL: {url.EscapeMarkup()}[/]");
            ProcessRunner.OpenUrl(url);
            return 0;
        }

        _console.MarkupLine("[red]No valid git repository found in the directory.[/]");
        return 1;
    }
}