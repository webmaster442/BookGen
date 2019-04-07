//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Services;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace BookGen.Editor.View
{
    internal class ReformatTableCommand: ICommand
    {
        private readonly EditorWrapper _editor;
        private readonly Regex _tableExpression;
        private readonly bool _padLeft;
        private const string _tablePattern = @"((?:(?:[^\n]*?\|[^\n]*)\ *)?(?:\r?\n|^))((?:\|\ *(?::?-+:?|::)\ *|\|?(?:\ *(?::?-+:?|::)\ *\|)+)(?:\ *(?::?-+:?|::)\ *)?\ *\r?\n)((?:(?:[^\n]*?\|[^\n]*)\ *(?:\r?\n|$))+)";

        public event EventHandler CanExecuteChanged;

        public ReformatTableCommand(EditorWrapper editor, bool padLeft)
        {
            _editor = editor;
            _padLeft = padLeft;
            _tableExpression = new Regex(_tablePattern, RegexOptions.Compiled);
        }

        public string ReformatTable(string input)
        {
            var rows = input.Split('\n');

            List<string[]> table = new List<string[]>(rows.Length);
            List<int[]> cellwidths = new List<int[]>();

            int colcount = 0;
            foreach (var row in rows)
            {
                var columns = from column in row.Trim().Split('|')
                              where column.Trim().Length > 0
                              select column.Trim();

                var lens = from column in columns
                           where column.Length > 0
                           select column.Length;

                if (!DividerRow(columns))
                {
                    var cols = columns.ToArray();
                    var lengths = lens.ToArray();
                    table.Add(cols);
                    cellwidths.Add(lengths);

                    if (lengths.Length > colcount)
                        colcount = lengths.Length;
                }
            }

            List<int> padsizes = new List<int>(colcount);
            for (int i=0; i<colcount; i++)
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

            StringBuilder result = new StringBuilder();

            int insetedRow = 0;
            for (int i=0; i<table.Count; ++i)
            {
                if (table[i] == null || table[i].Length < 1) continue;

                if (insetedRow == 1)
                {
                    WriteHeaderDivider(result, padsizes);
                    ++insetedRow;
                    continue;
                }

                result.Append("| ");
                string padded = null;
                for (int j=0; j<table[i].Length; ++j)
                {
                    if (_padLeft)
                        padded = table[i][j].PadLeft(padsizes[j]);
                    else
                        padded = table[i][j].PadRight(padsizes[j]);

                    result.Append(padded);
                    result.Append(" | ");
                }
                result.Append("\r\n");
                ++insetedRow;
            }

            return result.ToString();   
        }

        private void WriteHeaderDivider(StringBuilder result, List<int> padsizes)
        {
            result.Append("| ");
            string divider = null;
            foreach (var padsize in padsizes)
            {
                if (_padLeft)
                    divider = ":".PadLeft(padsize, '-');
                else
                    divider = ":".PadRight(padsize, '-');

                result.Append(divider);
                result.Append(" | ");
            }
            result.Append("\r\n");
        }

        private bool DividerRow(IEnumerable<string> columns)
        {
            return columns.Any(column => column.EndsWith("--:")) ||
                   columns.Any(column => column.StartsWith(":--"));
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var selectedText = _editor.Document.GetText(_editor.SelectionStart, _editor.SelectionLength);

            if (!_tableExpression.IsMatch(selectedText))
            {
                MessageBox.Show("Selected text isn't a table", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ExceptionHandler.SafeRun(() =>
            {
               var result = ReformatTable(selectedText);
               _editor.Document.Replace(_editor.SelectionStart, _editor.SelectionLength, result, OffsetChangeMappingType.RemoveAndInsert);
            });

        }
    }
}
