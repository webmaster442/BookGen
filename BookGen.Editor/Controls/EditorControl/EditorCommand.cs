//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit.Document;
using System;
using System.Windows.Input;

namespace BookGen.Editor.Controls
{
    internal class EditorCommand: ICommand
    {
        private readonly MarkdownEditor _editor;
        private readonly bool _tokenWrap;

        public EditorCommand(MarkdownEditor editor, bool tokenWrap)
        {
            _tokenWrap = tokenWrap;
            _editor = editor;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                //not used. Required by interface
            }
            remove
            {
                //not used. Required by interface
            }
        }

        private bool EditorHasSelection
        {
            get
            {
                return
                    _editor.SelectionStart > -1
                    && _editor.SelectionLength > 0
                    && (_editor.SelectionStart + _editor.SelectionLength) <= _editor.Document.TextLength;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string token = parameter as string;
            if (string.IsNullOrEmpty(token)) return;

            if (_tokenWrap)
                TokenWrap(token);
            else
                NoTokenWrap(token);

        }

        private void NoTokenWrap(string token)
        {
            if (_editor.SelectionLength == 0)
            {
                _editor.Document.Insert(_editor.SelectionStart, $"{token} ");
            }
            else
            {
                var selectedText = _editor.Document.GetText(_editor.SelectionStart, _editor.SelectionLength);
                if (selectedText.StartsWith(token))
                {
                    selectedText = selectedText.Substring(token.Length + 1);
                    _editor.Document.Replace(_editor.SelectionStart, _editor.SelectionLength, selectedText, OffsetChangeMappingType.RemoveAndInsert);
                }
                else
                {
                    selectedText = $"{token} {selectedText}";
                    _editor.Document.Replace(_editor.SelectionStart, _editor.SelectionLength, selectedText, OffsetChangeMappingType.RemoveAndInsert);
                }
            }
        }

        private bool CanContainTheToken(bool isHtmlToken, string token)
        {
            int tokenlen = token.Length * 2;

            if (isHtmlToken)
                ++tokenlen;

            return (_editor.SelectionStart + _editor.SelectionLength) >= tokenlen;
        }

        private string CreateEndToken(string token, bool isHtmlToken)
        {
            if (!isHtmlToken) return token;
            string tag = token.Substring(1, token.Length - 2);
            return $"</{tag}>";
        }

        private void TokenWrap(string token)
        {
            if (string.IsNullOrEmpty(token))
                return;

            bool isHtmlToken = token.IndexOf('<') == 0;

            var endTokenStr = CreateEndToken(token, isHtmlToken);

            if (EditorHasSelection && CanContainTheToken(isHtmlToken, token))
            {
                int endlen = isHtmlToken ? token.Length + 1 : token.Length;

                var starttoken = _editor.Document.GetText(_editor.SelectionStart, token.Length);
                var endtoken = _editor.Document.GetText(_editor.SelectionStart + _editor.SelectionLength - endlen, endlen);

                if (starttoken == token && endtoken == endTokenStr)
                {
                    _editor.Document.Remove(_editor.SelectionStart + _editor.SelectionLength - endlen, endlen);
                    _editor.Document.Remove(_editor.SelectionStart, token.Length);
                    return;
                }
            }
            if (_editor.SelectionLength == 0)
            {
                var newPos = _editor.SelectionStart;
                _editor.Document.Insert(_editor.SelectionStart, $"{token} {endTokenStr}");
                _editor.SelectionStart = newPos + token.Length;
                _editor.SelectionLength = 1;
            }
            else
            {
                _editor.Document.Insert(_editor.SelectionStart, token);
                _editor.Document.Insert(_editor.SelectionStart + _editor.SelectionLength, endTokenStr);
            }
        }
    }
}
