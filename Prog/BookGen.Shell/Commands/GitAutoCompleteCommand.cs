//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.DomainServices;
using BookGen.Shell.GitAutocomplete;

using Spectre.Console;

namespace BookGen.Shell.Commands;

[CommandName("git-complete")]
internal class GitAutoCompleteCommand : Command
{
    public override int Execute(string[] context)
    {
        var folder = Environment.CurrentDirectory;
        var items = GitCommandProvider.GetGitCommands(folder).Order().ToArray();

        if (context.Length == 2
            && int.TryParse(context[0], out int index)
            && !string.IsNullOrEmpty(context[1]))
        {
            var candidates = ShellAutoCompleteFilter.DoFilter(items, context[1], index);

            foreach (var candidate in candidates)
            {
                AnsiConsole.WriteLine(candidate);
            }
        }

        return 0;
    }
}
