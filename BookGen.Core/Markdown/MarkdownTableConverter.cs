//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace BookGen.Core.Markdown
{
    public class MarkdownTableConverter
    {
        public static bool TryConvertToMarkdownTable(string input, char delimiter, out string formatted)
        {
            List<string[]> table = ParseTable(input, delimiter);
            
            StringBuilder md = new StringBuilder();
            for (int i=0; i<table.Count; i++)
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
            for (int i=0; i<length; i++)
            {
                md.Append("| ");
                md.Append(":--");
                md.Append(" ");
            }
            md.Append("|\r\n");
        }

        private static void WriteRow(StringBuilder md, string[] columns)
        {
            foreach (var column in columns)
            {
                md.Append("| ");
                md.Append(column);
                md.Append(" ");
            }
            md.Append("|\r\n");
        }

        private static List<string[]> ParseTable(string input, char delimiter)
        {
            var lines = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string[]> table = new List<string[]>(lines.Length);
            foreach (var line in lines)
            {
                var columns = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                table.Add(columns);
            }
            return table;
        }
    }
}
