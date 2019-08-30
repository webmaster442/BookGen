//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using NHunspell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookGen.Editor.EditorControl
{
    internal sealed class SpellCheck
    {
        private const string _blockpatterns = "^[`]{3,}(.*)[`]{3,}|";
        private readonly Hunspell _hunspell;
        private readonly TextView _renderTarget;
        private readonly SpellingColorizer _spellingColorizer;
        private readonly Regex _codeBlockRegex;
        private readonly Regex _uriRegex;
        private readonly Regex _wordRegex;

        public SpellCheck(TextView target, Hunspell hunspell)
        {
            _renderTarget = target;
            _spellingColorizer = new SpellingColorizer();
            _codeBlockRegex = new Regex(_blockpatterns, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
            _uriRegex = new Regex("(http|ftp|https|mailto):\\/\\/[\\w\\-_]+(\\.[\\w\\-_]+)+([\\w\\-\\.,@?^=%&amp;:/~\\+#]*[\\w\\-\\@?^=%&amp;/~\\+#])?", RegexOptions.Compiled);
            _wordRegex = new Regex("-[^\\w]+|^'[^\\w]+|[^\\w]+'[^\\w]+|[^\\w]+-[^\\w]+|[^\\w]+'$|[^\\w]+-$|^-$|^'$|[^\\w'-]", RegexOptions.Compiled);

            _hunspell = hunspell;
        }

        public void DoSpellCheck()
        {
            if (_hunspell == null) return;
            if (!_renderTarget.VisualLinesValid) return;

            _spellingColorizer.Errors.Clear();

            foreach (VisualLine current in _renderTarget.VisualLines.AsParallel<VisualLine>())
            {
                int num = 0;
                string text = _renderTarget.Document.GetText(current.FirstDocumentLine.Offset, current.LastDocumentLine.EndOffset - current.FirstDocumentLine.Offset);
                if (!string.IsNullOrEmpty(text))
                {
                    text = Regex.Replace(text, "[\\u2018\\u2019\\u201A\\u201B\\u2032\\u2035]", "'");
                    string input = text;

                    input = _codeBlockRegex.Replace(input, string.Empty);
                    input = _uriRegex.Replace(input, string.Empty);
                    var words = _wordRegex.Split(input).Where(w => !string.IsNullOrEmpty(w));
                    foreach (string word in words)
                    {
                        string trimmed = word.Trim(new char[] { '\'', '_', '-' });
                        int trimcount = text.IndexOf(trimmed, num, System.StringComparison.InvariantCultureIgnoreCase);
                        if (trimcount > -1)
                        {
                            int start = current.FirstDocumentLine.Offset + trimcount;
                            if (!_hunspell.Spell(trimmed))
                            {
                                TextSegment item = new TextSegment
                                {
                                    StartOffset = start,
                                    Length = word.Length
                                };
                                _spellingColorizer.Errors.Add(item);
                            }
                            num = text.IndexOf(word, num, StringComparison.InvariantCultureIgnoreCase) + word.Length;
                        }
                    }
                }
            }
            _renderTarget.InvalidateLayer(_spellingColorizer.Layer);
        }

        public bool Spell(string word)
        {
            return _hunspell.Spell(word);
        }

        public List<string> Suggest(string word)
        {
            return _hunspell.Suggest(word);
        }
    }
}
