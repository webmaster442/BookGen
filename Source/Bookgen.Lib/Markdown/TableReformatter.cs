using System.Text;
using System.Text.RegularExpressions;

namespace Bookgen.Lib.Markdown;

public sealed partial class TableReformatter
{
    private enum PaddingMode
    {
        Left,
        Right,
    }

    public bool TryReformatMarkdownTable(string tableCode, out string reformatted)
    {
        if (!TablePattern().IsMatch(tableCode))
        {
            reformatted = tableCode;
            return false;
        }

        reformatted = ReformatTable(tableCode);
        return true;

    }

    private string ReformatTable(string input)
    {
        string[]? rows = input.Split('\n');

        var table = new List<string[]>(rows.Length);
        var cellwidths = new List<int[]>();
        PaddingMode paddingMode = PaddingMode.Left;

        int colcount = 0;
        foreach (string? row in rows)
        {
            IEnumerable<string>? columns = from column in row.Trim().Split('|')
                                           where column.Trim().Length > 0
                                           select column.Trim();

            IEnumerable<int>? columnLengths = from column in columns
                                              where column.Length > 0
                                              select column.Length;

            if (!ContainsDividerRow(columns))
            {
                string[]? cols = columns.ToArray();
                int[]? lengths = columnLengths.ToArray();
                table.Add(cols);
                cellwidths.Add(lengths);

                if (lengths.Length > colcount)
                    colcount = lengths.Length;
            }
            else
            {
                paddingMode = GetPaddingMode(columns);
            }
        }

        List<int> padsizes = CalculatePaddingSizes(table, colcount);

        return CreateResultMarkdownTable(table, paddingMode, padsizes);
    }

    private static List<int> CalculatePaddingSizes(List<string[]> table, int colcount)
    {
        var padsizes = new List<int>(colcount);
        for (int i = 0; i < colcount; i++)
        {
            int padsize = 0;
            foreach (string[]? row in table)
            {
                if (row == null || row.Length < 1) continue;

                if (row[i].Length > padsize)
                    padsize = row[i].Length;
            }
            padsizes.Add(padsize);
        }

        return padsizes;
    }

    private string CreateResultMarkdownTable(List<string[]> table,
                                             PaddingMode paddingMode,
                                             List<int> padsizes)
    {
        var result = new StringBuilder();

        int insetedRow = 0;
        for (int i = 0; i < table.Count; ++i)
        {
            if (table[i] == null || table[i].Length < 1) continue;

            if (insetedRow == 1)
            {
                WriteHeaderDivider(result, padsizes, paddingMode);
                ++insetedRow;
            }

            result.Append("| ");
            for (int j = 0; j < table[i].Length; ++j)
            {
                string padded;
                if (paddingMode == PaddingMode.Left)
                    padded = table[i][j].PadRight(padsizes[j]);
                else
                    padded = table[i][j].PadLeft(padsizes[j]);

                result.Append(padded);
                result.Append(" |");
                if (j != table[i].Length - 1)
                {
                    result.Append(' ');
                }
            }
            result.AppendLine();
            ++insetedRow;
        }

        return result.ToString();
    }

    private PaddingMode GetPaddingMode(IEnumerable<string> columns)
    {
        int right = columns.Count(column => column.EndsWith("--:"));
        int left = columns.Count(column => column.StartsWith(":--"));

        return left < right 
            ? PaddingMode.Right 
            : PaddingMode.Left;
    }

    private static void WriteHeaderDivider(StringBuilder result,
                                           List<int> padsizes,
                                           PaddingMode paddingMode)
    {
        result.Append("| ");
        for (int i = 0; i < padsizes.Count; i++)
        {
            int padsize = padsizes[i];
            string divider;
            if (paddingMode == PaddingMode.Left)
                divider = ":".PadRight(padsize, '-');
            else
                divider = ":".PadLeft(padsize, '-');

            result.Append(divider);
            result.Append(" |");
            if (i != padsizes.Count - 1)
            {
                result.Append(' ');
            }
        }
        result.AppendLine();
    }

    private static bool ContainsDividerRow(IEnumerable<string> columns)
    {
        return columns.Any(column => column.EndsWith("--:")) ||
               columns.Any(column => column.StartsWith(":--"));
    }

    [GeneratedRegex(@"((?:(?:[^\n]*?\|[^\n]*)\ *)?(?:\r?\n|^))((?:\|\ *(?::?-+:?|::)\ *|\|?(?:\ *(?::?-+:?|::)\ *\|)+)(?:\ *(?::?-+:?|::)\ *)?\ *\r?\n)((?:(?:[^\n]*?\|[^\n]*)\ *(?:\r?\n|$))+)", RegexOptions.Compiled)]
    private static partial Regex TablePattern();
}
