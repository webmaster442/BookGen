//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit.Document;
using System;
using System.Windows.Input;

namespace BookGen.Launch.Controls
{
    internal class EditCommand : ICommand
    {
        private readonly MarkdownEditor _editor;
        private readonly bool _wrapLeft;
        private readonly bool _wrapRight;

        public EditCommand(MarkdownEditor editor, bool wrapLeft, bool wrapRight)
        {
            _editor = editor;
            _wrapLeft = wrapLeft;
            _wrapRight = wrapRight;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual void Execute(object? parameter)
        {
            if (parameter is not string symbols)
            {
                return;
            }

            if (_wrapLeft && TryGetSelectedText(out string selected))
            {
                var newText = $"{symbols} {selected}";

                if (_wrapRight)
                    newText = $"{symbols} {selected} {symbols}";
                   
                ReplaceSelectedText(newText);
            }
            else
            {
                IsertTextAtCarret(symbols);
            }
        }

        protected void IsertTextAtCarret(string symbols)
        {
            _editor.Document.Insert(_editor.SelectionStart, symbols);
        }

        protected void ReplaceSelectedText(string selected)
        {
            _editor.Document.Replace(_editor.SelectionStart, _editor.SelectionLength, selected, OffsetChangeMappingType.RemoveAndInsert);
        }

        protected bool TryGetSelectedText(out string selected)
        {
            if (_editor.SelectionStart > -1
                && _editor.SelectionLength > 0
                && (_editor.SelectionStart + _editor.SelectionLength) <= _editor.Document.TextLength)
            {
                selected = _editor.Document.GetText(_editor.SelectionStart, _editor.SelectionLength);
                return true;
            }
            selected = string.Empty;
            return false;
        }
    }
}
