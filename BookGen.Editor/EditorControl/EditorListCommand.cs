//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit.Document;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace BookGen.Editor.EditorControl
{
    internal class EditorListCommand : ICommand
    {
        private readonly MarkdownEditor _editor;
        private readonly bool _ordered;
        private readonly Regex _numbering;

        public EditorListCommand(MarkdownEditor editor, bool ordered)
        {
            _editor = editor;
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
            if (_editor.SelectionLength == 0)
            {
                if (_ordered)
                    _editor.Document.Insert(_editor.SelectionStart, "1. ");
                else
                    _editor.Document.Insert(_editor.SelectionStart, "- ");
            }
            else
            {
                var SelectedText = _editor.Document.GetText(_editor.SelectionStart, _editor.SelectionLength);
                var lines = SelectedText.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (_ordered)
                        lines[i] = OrderedList(i, lines[i].Trim());
                    else
                        lines[i] = UnorderedList(lines[i].Trim());
                }
                var text = string.Join("\r\n", lines);
                _editor.Document.Replace(_editor.SelectionStart, _editor.SelectionLength, text, OffsetChangeMappingType.RemoveAndInsert);
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
