﻿//-----------------------------------------------------------------------------
// (c) 2021-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.DomainServices.Markdown
{
    public sealed partial class FootNoteReindexer
    {
        private readonly StringBuilder _footnotes;
        private readonly StringBuilder _regulartext;
        private readonly ILogger _log;
        private readonly bool _appendLineBreakbeforeDefs;
        private int _counter;

        [GeneratedRegex(".\\[\\^\\d+\\]")]
        private static partial Regex FootNoteRef();


        [GeneratedRegex("\\[\\^\\d+\\]\\:")]
        private static partial Regex FootNoteDef();


        public FootNoteReindexer(ILogger log, bool appendLineBreakbeforeDefs = false)
        {
            _regulartext = new StringBuilder(64*1024);
            _footnotes = new StringBuilder(8*1024);
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

        public void AddHtml(string content)
        {
            _regulartext.Append(content);
        }

        public void AddMarkdown(string document)
        {
            int currentDocLimit = 0;
            MatchCollection? referenceMatches = FootNoteRef().Matches(document);
            Match? lastDefinition = FootNoteRef().Matches(document).LastOrDefault();

            if (referenceMatches.Count == 0 && lastDefinition == null)
            {
                //document doesn't contain footnotes
                _regulartext.Append(document);
                return;
            }

            if (lastDefinition?.Length > 3)
            {
                string? numberString = lastDefinition.Value[3..^1];
                currentDocLimit = int.Parse(numberString);

                if (currentDocLimit != referenceMatches.Count)
                {
                    _log.LogWarning("Expected {limit} footnotes. Found: {count}", currentDocLimit, referenceMatches.Count);
                    return;
                }
            }

            Match? definitionStart = FootNoteDef().Matches(document).FirstOrDefault();
            if (definitionStart != null)
            {
                var regular = new StringBuilder(document[..definitionStart.Index]);
                var footnote = new StringBuilder(document[definitionStart.Index..]);
                DoReindexing(currentDocLimit, regular, footnote);
                _regulartext.Append(regular);
                _regulartext.AppendLine();
                _footnotes.Append(footnote);
                _footnotes.AppendLine();
                _counter += currentDocLimit;
            }
        }

        private void DoReindexing(int currentDocLimit, StringBuilder regular, StringBuilder footnote)
        {
            for (int i = currentDocLimit - 1; i >= 0; i--)
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
