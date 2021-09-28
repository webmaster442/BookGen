//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Core.Markdown
{
    public class TableReformatter
    {
        private enum PaddingMode
        {
            Left,
            Right,
        }

        private readonly Regex _tableExpression;
        private const string _tablePattern = @"((?:(?:[^\n]*?\|[^\n]*)\ *)?(?:\r?\n|^))((?:\|\ *(?::?-+:?|::)\ *|\|?(?:\ *(?::?-+:?|::)\ *\|)+)(?:\ *(?::?-+:?|::)\ *)?\ *\r?\n)((?:(?:[^\n]*?\|[^\n]*)\ *(?:\r?\n|$))+)";

        public TableReformatter()
        {
            _tableExpression = new Regex(_tablePattern, RegexOptions.Compiled);
        }

        public bool TryReformatMarkdownTable(string tableCode, out string reformatted)
        {
            if (!_tableExpression.IsMatch(tableCode))
            {
                reformatted = tableCode;
                return false;
            }

            reformatted = ReformatTable(tableCode);
            return true;

        }

        private string ReformatTable(string input)
        {
            var rows = input.Split('\n');

            List<string[]> table = new List<string[]>(rows.Length);
            List<int[]> cellwidths = new List<int[]>();
            PaddingMode paddingMode = PaddingMode.Left;

            int colcount = 0;
            foreach (var row in rows)
            {
                var columns = from column in row.Trim().Split('|')
                              where column.Trim().Length > 0
                              select column.Trim();

                var columnLengths = from column in columns
                                    where column.Length > 0
                                    select column.Length;

                if (!ContainsDividerRow(columns))
                {
                    var cols = columns.ToArray();
                    var lengths = columnLengths.ToArray();
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
            List<int> padsizes = new List<int>(colcount);
            for (int i = 0; i < colcount; i++)
            {
                int padsize = 0;
                foreach (var row in table)
                {
                    if (row == null || row.Length < 1) continue;

                    if (row[i].Length > padsize)
                        padsize = row[i].Length;
                }
                padsizes.Add(padsize);
            }

            return padsizes;
        }

        private string CreateResultMarkdownTable(List<string[]> table, PaddingMode paddingMode, List<int> padsizes)
        {
            StringBuilder result = new StringBuilder();

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
                    result.Append(" | ");
                }
                result.Append("\r\n");
                ++insetedRow;
            }

            return result.ToString();
        }

        private PaddingMode GetPaddingMode(IEnumerable<string> columns)
        {
            int right = columns.Count(column => column.EndsWith("--:"));
            int left = columns.Count(column => column.StartsWith(":--"));

            if (left < right)
                return PaddingMode.Right;
            else
                return PaddingMode.Left;
        }

        private void WriteHeaderDivider(StringBuilder result, List<int> padsizes, PaddingMode paddingMode)
        {
            result.Append("| ");
            string divider = string.Empty;
            foreach (var padsize in padsizes)
            {
                if (paddingMode == PaddingMode.Left)
                    divider = ":".PadRight(padsize, '-');
                else
                    divider = ":".PadLeft(padsize, '-');

                result.Append(divider);
                result.Append(" | ");
            }
            result.Append("\r\n");
        }

        private bool ContainsDividerRow(IEnumerable<string> columns)
        {
            return columns.Any(column => column.EndsWith("--:")) ||
                   columns.Any(column => column.StartsWith(":--"));
        }
    }
}
