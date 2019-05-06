//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Services;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using NHunspell;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace BookGen.Editor.View
{
    internal sealed class SpellingErrorColorizer : DocumentColorizingTransformer, IDisposable
    {
        private readonly EditorWrapper _editor;
        private readonly TextDecorationCollection collection;
        private Hunspell _hunspell;
        private readonly Regex _sanitizer;
        private const string _sanitizePattern = @"(\(|\)|\[|\]|\{|\}|\.|\?|\!|\<|\>|\#|\,|\;|\:|\-|\|)";

        public SpellingErrorColorizer(EditorWrapper editor, Hunspell hunspell)
        {
            collection = new TextDecorationCollection();
            var dec = new TextDecoration();
            dec.Pen = new Pen { Thickness = 1, DashStyle = DashStyles.DashDot, Brush = new SolidColorBrush(Colors.Red) };
            dec.PenThicknessUnit = TextDecorationUnit.FontRecommended;
            collection.Add(dec);
            _sanitizer = new Regex(_sanitizePattern, RegexOptions.Compiled);

            _editor = editor;
            _hunspell = hunspell;
        }

        ~SpellingErrorColorizer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_hunspell != null)
            {
                _hunspell.Dispose();
                _hunspell = null;
            }
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            string lineText = _editor.Document.GetText(line);

            lineText = _sanitizer.Replace(lineText, " ");
            var words = EditorServices.GetWords(lineText);

            foreach (var word in words)
            {
                if (string.IsNullOrEmpty(word.Value)) continue;
                bool correct = _hunspell.Spell(word.Value);
                if (!correct)
                {
                    var wordEnd = word.Key + line.Offset + word.Value.Length;
                    ChangeLinePart(word.Key + line.Offset, wordEnd, (VisualLineElement element) =>
                      {
                          element.TextRunProperties.SetTextDecorations(collection);
                      });
                }
            }
        }
    }
}
