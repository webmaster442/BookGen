//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;
using System.Xml.Serialization;

using Spectre.Console;

namespace BookGen.Infrastructure.Terminal;

public static class Terminal
{
    private static readonly Palette _palette = new();

    public static void Table(string[] headers, IEnumerable<string[]> rows)
    {
        var table = new Table();
        table.AddColumns(headers);

        foreach (var row in rows)
        {
            table.AddRow(row);
        }

        AnsiConsole.Write(table);
    }

    public static void BarChart(IDictionary<string, double> items, string title = "")
    {
        BarChart chart = new BarChart()
            .Width(Console.WindowWidth)
            .Label(title)
            .CenterLabel();

        _palette.Reset();

        foreach (KeyValuePair<string, double> item in items)
        {
            chart.AddItem(item.Key, item.Value, _palette.GetNextColor());
        }

        AnsiConsole.Write(chart);
    }

    public static void BreakDownChart(IDictionary<string, double> items, string title, bool descendingOrder = true)
    {
        Rule rule = new Rule(title).Centered();
        AnsiConsole.Write(rule);

        BreakdownChart chart = new BreakdownChart()
            .Width(Console.WindowWidth);

        _palette.Reset();

        IEnumerable<KeyValuePair<string, double>> data = descendingOrder
            ? items.OrderByDescending(x => x.Value)
            : items;

        foreach (KeyValuePair<string, double> item in data)
        {
            chart.AddItem(item.Key, item.Value, _palette.GetNextColor());
        }

        AnsiConsole.Write(chart);
    }

    public static void Header(string title, int blankLineBefore = 0, int blankLineAfter = 1)
    {
        for (int i = 0; i < blankLineBefore; i++)
            AnsiConsole.WriteLine();
        AnsiConsole.WriteLine(title);
        AnsiConsole.WriteLine("".PadLeft(Console.WindowWidth, '-'));
        for (int i = 0; i < blankLineAfter; i++)
            AnsiConsole.WriteLine();
    }

    public static bool Confirm(string message)
    {
        var prompt = new ConfirmationPrompt(message);
        return prompt.Show(AnsiConsole.Console);
    }

    public static List<T> SelectionMenu<T>(IEnumerable<T> items, string title, string instructions, Func<T, string> displaySelector) where T : notnull
    {
        MultiSelectionPrompt<T> prompt = new MultiSelectionPrompt<T>()
            .Title(title)
            .PageSize(15)
            .InstructionsText(instructions)
            .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
            .AddChoices(items)
            .UseConverter(displaySelector);

        return prompt.Show(AnsiConsole.Console);
    }

    public static void List(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            AnsiConsole.WriteLine($"* {item}");
        }
    }
}
