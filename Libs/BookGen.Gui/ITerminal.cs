namespace BookGen.Gui;

public interface ITerminal
{
    void BarChart(IDictionary<string, double> items, string title = "");
    void BreakDownChart(IDictionary<string, double> items, string title = "");
    void Header(string title);
    void Table<T>(IDictionary<string, T> rows, string keyColumnName = "", string valueColumnName = "");
    void Table<T>(IEnumerable<T> rows);
}