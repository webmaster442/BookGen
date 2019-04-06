//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit.Document;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace BookGen.Editor.View
{
    internal class EditorListCommand: ICommand
    {
        private readonly EditorWrapper _e;
        private readonly bool _ordered;
        private readonly Regex _numbering; 

        public EditorListCommand(EditorWrapper editor, bool ordered)
        {
            _e = editor;
            _ordered = ordered;
            _numbering = new Regex(@"(\d+)(\.\s)", RegexOptions.Compiled);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_e.SelectionLength == 0)
            {
                if (_ordered)
                    _e.Document.Insert(_e.SelectionStart, "1. ");
                else
                    _e.Document.Insert(_e.SelectionStart, "- ");
            }
            else
            {
                var SelectedText = _e.Document.GetText(_e.SelectionStart, _e.SelectionLength);
                var lines = SelectedText.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (_ordered)
                        lines[i] = OrderedList(i, lines[i].Trim());
                    else
                        lines[i] = UnorderedList(lines[i].Trim());
                }
                var text = string.Join("\r\n", lines);
                _e.Document.Replace(_e.SelectionStart, _e.SelectionLength, text, OffsetChangeMappingType.RemoveAndInsert);
            }
        }

        private string OrderedList(int i, string line)
        {
            if (_numbering.IsMatch(line))
                line = _numbering.Replace(line, "");
            else
                line = $"{i + 1}. {line}";
            return line;
        }

        private string UnorderedList(string line)
        {
            if (line.StartsWith("- ")) line = line.Substring(2);
            else line = $"- {line}";
            return line;
        }
    }
}
