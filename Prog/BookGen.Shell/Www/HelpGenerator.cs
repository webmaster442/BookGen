//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Www;

using Spectre.Console;

namespace BookGen.Shell.Www;

internal static class HelpGenerator
{
    public static void PrintHelp(IEnumerable<WwwBang> bangs)
    {
        AnsiConsole.MarkupLine("[bold]Supported bangs[/]");
        AnsiConsole.WriteLine();

        var table = new Table();
        table.AddColumns("Site", "Activator(s)");
        foreach (var bang in bangs)
        {
            table.AddRow($"[green]{bang.Value.EscapeMarkup()}[/]", GetActivators(bang));
        }
        AnsiConsole.Write(table);
    }

    private static string GetActivators(WwwBang bang)
    {
        return string.IsNullOrEmpty(bang.AltActivator)
            ? $"[italic]{bang.Activator.EscapeMarkup()}[/]"
            : $"[italic]{bang.Activator.EscapeMarkup()}[/] or [italic]{bang.AltActivator.EscapeMarkup()}[/]";
    }
}
