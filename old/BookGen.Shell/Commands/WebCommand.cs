//-----------------------------------------------------------------------------
// (c) 2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using System.Text.RegularExpressions;

using BookGen.Cli.Annotations;
using BookGen.DomainServices;

using Spectre.Console;

namespace BookGen.Shell.Commands;

[CommandName("web")]
internal sealed partial class WebCommand : GitCommandBase
{
    [GeneratedRegex("^git@(.+)\\:(.+)\\.git$")]
    private static partial Regex SshRemotePattern();

    [GeneratedRegex("^(http://|https://)(.+).git$")]
    private static partial Regex HttpRemotePattern();

    public WebCommand(IAnsiConsole console) : base(console)
    {
    }

    private static bool TryParseRemoteToUrl(string remote, out string url)
    {
        if (SshRemotePattern().IsMatch(remote))
        {
            url = SshRemotePattern().Replace(remote, "https://$1/$2");
            return true;
        }

        if (HttpRemotePattern().IsMatch(remote))
        {
            url = HttpRemotePattern().Replace(remote, "$1$2");
            return true;
        }

        url = string.Empty;
        return false;
    }

    public override int Execute(GitArguments arguments, string[] context)
    {
        if (string.IsNullOrEmpty(arguments.WorkDirectory))
            arguments.WorkDirectory = Environment.CurrentDirectory;

        if (!string.IsNullOrEmpty(arguments.WorkDirectory)
            && TestIfGitDir(arguments.WorkDirectory))
        {
            var remote = GetGitRemote(arguments.WorkDirectory);
            if (string.IsNullOrEmpty(remote))
            {
                _console.MarkupLine("[red]No remote repository found[/]");
                return 1;
            }

            if (TryParseRemoteToUrl(remote, out string url))
            {
                _console.MarkupLine($"Opening [blue]{url}[/]...");
                ProcessRunner.OpenUrl(url);
                return 0;
            }

            _console.MarkupLine("[red]Cannot parse remote url[/]");
            return 0;
        }
        _console.MarkupLine("[red]Not a git repository[/]");
        return 1;
    }
}
