using System.Text;

namespace Bookgen.Lib.Markdown;

public static class MarkdownTableConverter
{
    public static bool TryConvertToMarkdownTable(string input, char delimiter, out string formatted)
    {
        List<string[]> table = ParseTable(input, delimiter);

        var md = new StringBuilder();
        for (int i = 0; i < table.Count; i++)
        {
            WriteRow(md, table[i]);
            if (i == 0)
            {
                WriteHeaderDivider(md, table[0].Length);
            }
        }

        var tableReformatter = new TableReformatter();

        return tableReformatter.TryReformatMarkdownTable(md.ToString(), out formatted);
    }

    private static void WriteHeaderDivider(StringBuilder md, int length)
    {
        for (int i = 0; i < length; i++)
        {
            md.Append("| ");
            md.Append(":--");
            md.Append(' ');
        }
        md.Append('|');
        md.AppendLine();
    }

    private static void WriteRow(StringBuilder md, string[] columns)
    {
        foreach (string? column in columns)
        {
            md.Append("| ");
            md.Append(column);
            md.Append(' ');
        }
        md.AppendLine();
    }

    private static List<string[]> ParseTable(string input, char delimiter)
    {
        string[]? lines = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        var table = new List<string[]>(lines.Length);
        foreach (string? line in lines)
        {
            string[]? columns = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            table.Add(columns);
        }
        return table;
    }
}
