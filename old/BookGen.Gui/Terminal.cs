using System.Reflection;

using Spectre.Console;

namespace BookGen.Gui;
public class Terminal : ITerminal
{
    private readonly Palette _palette;

    public Terminal()
    {
        _palette = new Palette();
    }

    public void Table<T>(IDictionary<string, T> rows,
                         string keyColumnName = "",
                         string valueColumnName = "")
    {
        var table = new Table();
        table.AddColumn(keyColumnName);
        table.AddColumn(valueColumnName);

        foreach (var row in rows)
        {
            table.AddRow(row.Key, row.Value?.ToString() ?? string.Empty);
        }

        AnsiConsole.Write(table);
    }

    public void Table<T>(IEnumerable<T> rows)
    {
        var table = new Table();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
            table.AddColumn(property.Name);

        foreach (var row in rows)
        {
            string[] values = properties.Select(x => x.GetValue(row)?.ToString() ?? string.Empty).ToArray();
            table.AddRow(values);
        }

        AnsiConsole.Write(table);
    }

    public void BarChart(IDictionary<string, double> items, string title = "")
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

    public void BreakDownChart(IDictionary<string, double> items, string title = "")
    {
        var rule = new Rule(title).Centered();
        AnsiConsole.Write(rule);

        var chart = new BreakdownChart()
            .Width(Console.WindowWidth);

        _palette.Reset();

        foreach (var item in items)
        {
            chart.AddItem(item.Key, item.Value, _palette.GetNextColor());
        }

        AnsiConsole.Write(chart);
    }

    public void Header(string title)
    {
        AnsiConsole.WriteLine(title);
        AnsiConsole.WriteLine("".PadLeft(Console.WindowWidth, '-'));
        AnsiConsole.WriteLine();
    }

    public bool Confirm(string message)
    {
        var prompt = new ConfirmationPrompt(message);
        return prompt.Show(AnsiConsole.Console);
    }
}
