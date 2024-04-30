//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

using BookGen.DomainServices;

using Spectre.Console;

namespace BookGen.Shell.GitGui;

internal sealed partial class CloneCommand : GuiCommand
{
    private readonly IAnsiConsole _console;
    [GeneratedRegex(@"(?<host>(git@|https://)([\w\.@]+)(/|:))(?<owner>[\w,\-,\\_]+)/(?<repo>[\w,\-,\\_]+)(.git){0,1}((/){0,1})")]
    private static partial Regex GitRepoRegex();

    public override string DisplayName => "Clone";

    public CloneCommand(IAnsiConsole console)
    {
        _console = console;
    }

    public override int Execute(string workDir, IProgress<string> progress)
    {
        var url = _console.Ask<string>("Enter the URL of the repository to clone: ");
        if (!GitRepoRegex().IsMatch(url))
        {
            _console.MarkupLine($"[red]Invalid URL: {url.EscapeMarkup()}[/]");
            return GitGuiCodes.ErrorCloneInvalidUrl;
        }
        return ProcessRunner.RunProcess("git", ["clone", "--progress", url], workDir, progress);
    }
}
