//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Core.Markdown
{
    public class FootNoteReindexer
    {
        private readonly StringBuilder _footnotes;
        private readonly StringBuilder _regulartext;
        private readonly static Regex footnoteRef = new Regex(@"\[\^\d+\]");
        private readonly static Regex footnoteDef = new Regex(@"\[\^\d+\]:");
        private int _counter;


        public FootNoteReindexer()
        {
            _regulartext = new StringBuilder(8096);
            _footnotes = new StringBuilder(4096);
            _counter = 0;
        }

        public void Clear()
        {
            _footnotes.Clear();
            _regulartext.Clear();
            _counter = 0;
        }

        public void AddMarkdown(string document)
        {
            int currentDocLimit = 0;
            var referenceMatches = footnoteRef.Matches(document);
            var lastDefinition = footnoteRef.Matches(document).LastOrDefault();

            if (referenceMatches.Count == 0 && lastDefinition == null)
            {
                //document doesn't contain footnotes
                _regulartext.Append(document);
                return;
            }

            if (lastDefinition != null)
            {
                var numberString = lastDefinition.Value[2..^1];
                currentDocLimit = int.Parse(numberString);

                if (currentDocLimit != referenceMatches.Count)
                {
                    //Log this
                    return;
                }
            }

            var definitionStart = footnoteDef.Matches(document).FirstOrDefault();
            if (definitionStart != null)
            {
                var regular = new StringBuilder(document.Substring(0, definitionStart.Index));
                var footnote = new StringBuilder(document.Substring(definitionStart.Index));
                DoReindexing(currentDocLimit, regular, footnote);
                _regulartext.Append(regular);
                _footnotes.Append(footnote);
                _counter += currentDocLimit;
            }
        }

        private void DoReindexing(int currentDocLimit, StringBuilder regular, StringBuilder footnote)
        {
            for (int i = 0; i < currentDocLimit; i++)
            {
                int indexToReplace = i + 1;
                int targetIndex = _counter + i + 1;

                regular.Replace($"[^{indexToReplace}]", $"[^{targetIndex}]");
                footnote.Replace($"[{indexToReplace}]:", $"[{targetIndex}]:");
            }
        }

        public override string ToString()
        {
            return $"{_regulartext}{_footnotes}";
        }
    }
}
