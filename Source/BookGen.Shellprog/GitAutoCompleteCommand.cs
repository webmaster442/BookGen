//#define DEBUGGING

//-----------------------------------------------------------------------------
// (c) 2024-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Spectre.Console;

namespace BookGen.Shellprog;

[CommandName("git-complete")]
internal sealed class GitAutoCompleteCommand : Command
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

#if DEBUGGING
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                items = items,
                context = context,
                candidates = candidates.ToArray(),
            },
            new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
            });
            File.WriteAllText($"gitcomplete-{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.json", json);
#endif

            foreach (var candidate in candidates)
            {
                AnsiConsole.WriteLine(candidate);
            }
        }

        return 0;
    }
}
