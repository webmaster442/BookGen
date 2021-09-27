//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Core.Markdown
{
    public class FootNoteReindexer
    {
        private readonly StringBuilder _footnotes;
        private readonly StringBuilder _regulartext;
        private readonly static Regex footnoteRef = new Regex(@".\[\^\d+\]");
        private readonly static Regex footnoteDef = new Regex(@"\[\^\d+\]\:");
        private readonly ILog _log;
        private readonly bool _appendLineBreakbeforeDefs;
        private int _counter;


        public FootNoteReindexer(ILog log, bool appendLineBreakbeforeDefs = false)
        {
            _regulartext = new StringBuilder(8096);
            _footnotes = new StringBuilder(4096);
            _counter = 0;
            _log = log;
            _appendLineBreakbeforeDefs = appendLineBreakbeforeDefs;
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

            if (lastDefinition != null 
                && lastDefinition.Length > 3)
            {
                var numberString = lastDefinition.Value[3..^1];
                currentDocLimit = int.Parse(numberString);

                if (currentDocLimit != referenceMatches.Count)
                {
                    _log.Warning("Expected {0} footnotes. Found: {0}", currentDocLimit, referenceMatches.Count);
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
            for (int i=currentDocLimit-1; i>=0; i--)
            {
                int indexToReplace = i + 1;
                int targetIndex = _counter + i + 1;

                regular.Replace($"[^{indexToReplace}]", $"[^{targetIndex}]");
                if (_appendLineBreakbeforeDefs)
                    footnote.Replace($"[^{indexToReplace}]:", $"\r\n[^{targetIndex}]:");
                else
                    footnote.Replace($"[^{indexToReplace}]:", $"[^{targetIndex}]:");
            }
        }

        public override string ToString()
        {
            return $"{_regulartext}{_footnotes}";
        }
    }
}
