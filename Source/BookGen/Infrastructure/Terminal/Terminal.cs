using System.Reflection;

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
        var chart = new BarChart()
            .Width(Console.WindowWidth)
            .Label(title)
            .CenterLabel();

        _palette.Reset();

        foreach (var item in items)
        {
            chart.AddItem(item.Key, item.Value, _palette.GetNextColor());
        }

        AnsiConsole.Write(chart);
    }

    public static void BreakDownChart(IDictionary<string, double> items, string title, bool descendingOrder = true)
    {
        var rule = new Rule(title).Centered();
        AnsiConsole.Write(rule);

        var chart = new BreakdownChart()
            .Width(Console.WindowWidth);

        _palette.Reset();

        IEnumerable<KeyValuePair<string, double>> data = descendingOrder
            ? items.OrderByDescending(x => x.Value)
            : items;

        foreach (var item in data)
        {
            chart.AddItem(item.Key, item.Value, _palette.GetNextColor());
        }

        AnsiConsole.Write(chart);
    }

    public static void Header(string title)
    {
        AnsiConsole.WriteLine(title);
        AnsiConsole.WriteLine("".PadLeft(Console.WindowWidth, '-'));
        AnsiConsole.WriteLine();
    }

    public static bool Confirm(string message)
    {
        var prompt = new ConfirmationPrompt(message);
        return prompt.Show(AnsiConsole.Console);
    }

    internal static List<T> SelectionMenu<T>(IEnumerable<T> items, string title, string instructions, Func<T, string> displaySelector) where T: notnull
    {
        var prompt = new MultiSelectionPrompt<T>()
            .Title(title)
            .PageSize(15)
            .InstructionsText(instructions)
            .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
            .AddChoices(items)
            .UseConverter(displaySelector);

        return prompt.Show(AnsiConsole.Console);
    }
}
