//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit.Document;
using System;
using System.Windows.Input;

namespace BookGen.Editor.View
{
    internal class EditorCommand : ICommand
    {
        private readonly EditorWrapper _e;
        private readonly bool _tokenWrap;

        public EditorCommand(EditorWrapper editor, bool tokenWrap)
        {
            _tokenWrap = tokenWrap;
            _e = editor;
        }

        public bool EditorHasSelection
        {
            get
            {
                return
                    _e.SelectionStart > -1
                    && _e.SelectionLength > 0
                    && (_e.SelectionStart + _e.SelectionLength) <= _e.Document.TextLength;
            }
        }

        public event EventHandler CanExecuteChanged;

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
            if (_e.SelectionLength == 0)
            {
                _e.Document.Insert(_e.SelectionStart, $"{token} ");
            }
            else
            {
                var selectedText = _e.Document.GetText(_e.SelectionStart, _e.SelectionLength);
                if (selectedText.StartsWith(token))
                {
                    selectedText = selectedText.Substring(token.Length + 1);
                    _e.Document.Replace(_e.SelectionStart, _e.SelectionLength, selectedText, OffsetChangeMappingType.RemoveAndInsert);
                }
                else
                {
                    selectedText = $"{token} {selectedText}";
                    _e.Document.Replace(_e.SelectionStart, _e.SelectionLength, selectedText, OffsetChangeMappingType.RemoveAndInsert);
                }
            }
        }

        private bool CanContainTheToken(bool isHtmlToken, string token)
        {
            int tokenlen = token.Length * 2;

            if (isHtmlToken)
                ++tokenlen;

            return (_e.SelectionStart + _e.SelectionLength) >= tokenlen;
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

                var starttoken = _e.Document.GetText(_e.SelectionStart, token.Length);
                var endtoken = _e.Document.GetText(_e.SelectionStart + _e.SelectionLength - endlen, endlen);

                if (starttoken == token && endtoken == endTokenStr)
                {
                    _e.Document.Remove(_e.SelectionStart + _e.SelectionLength - endlen, endlen);
                    _e.Document.Remove(_e.SelectionStart, token.Length);
                    return;
                }
            }
            if (_e.SelectionLength == 0)
            {
                var newPos = _e.SelectionStart;
                _e.Document.Insert(_e.SelectionStart, $"{token} {endTokenStr}");
                _e.SelectionStart = newPos + token.Length;
                _e.SelectionLength = 1;
            }
            else
            {
                _e.Document.Insert(_e.SelectionStart, token);
                _e.Document.Insert(_e.SelectionStart + _e.SelectionLength, endTokenStr);
            }
        }
    }
}
