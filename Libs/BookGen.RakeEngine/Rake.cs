//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.RakeEngine.Internals;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Syntax;
using System.Globalization;
using System.Text;

namespace BookGen.RakeEngine;

public sealed class Rake
{
    private readonly int _minCharLength;
    private readonly int _maxWordsLength;
    private readonly double _minKeywordFrequency;
    private readonly HashSet<string> _stopWords;

    public Rake(CultureInfo stopWordCulture,
                int minCharLength = 1,
                int maxWordsLength = 5,
                double minKeywordFrequency = 1)
    {
        _minCharLength = minCharLength;
        _maxWordsLength = maxWordsLength;
        _minKeywordFrequency = minKeywordFrequency;
        _stopWords = StopwordsLoader.GetStopWords(stopWordCulture);
    }

    public Dictionary<string, float> RunMarkdown(string markdown)
    {
        var markdownContent = new StringBuilder(markdown.Length);
        MarkdownPipeline? pipeline = new MarkdownPipelineBuilder().UseAutoIdentifiers(AutoIdentifierOptions.GitHub).Build();
        MarkdownDocument doc = Markdig.Markdown.Parse(markdown, pipeline);

        foreach (MarkdownObject item in doc.Descendants())
        {
            if (item is HeadingBlock heading)
            {
                string? txt = heading.Inline?.FirstChild?.ToString();
                markdownContent.AppendLine(txt);
            }
            else if (item is ParagraphBlock paragraph)
            {
                string? text = paragraph.Inline?.FirstChild?.ToString();
                markdownContent.AppendLine(text);
            }
        }

        return Run(markdownContent.ToString());
    }

    private Dictionary<string, float> Run(string text)
    {
        string[] sentenceList = RakeHelpers.SplitSentences(text.ToLowerInvariant());

        List<string> phraseList = GenerateCandidateKeywords(sentenceList, _minCharLength, _maxWordsLength);

        Dictionary<string, float> wordScores = RakeHelpers.CalculateWordScores(phraseList);

        Dictionary<string, float> keywordCandidates = RakeHelpers.GenerateCandidateKeywordScores(phraseList, wordScores, _minKeywordFrequency);

        return keywordCandidates
            .Where(pair => !_stopWords.Contains(pair.Key))
            .OrderByDescending(pair => pair.Value)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private List<string> GenerateCandidateKeywords(string[] sentenceList,
                                                   int minCharLength,
                                                   int maxWordsLength)
    {
        var phraseList = new List<string>();

        var sb = new StringBuilder();

        foreach (string sentence in sentenceList)
        {
            string sLowerCase = sentence.Trim();

            var wordSplitter = new StringSplitter(sLowerCase.AsSpan(), ' ');

            while (wordSplitter.TryGetNext(out ReadOnlySpan<char> wordSpan))
            {
                string word = wordSpan.ToString();

                if (_stopWords.Contains(word))
                {
                    string phrase = sb.ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(phrase)
                        && RakeHelpers.IsAcceptable(phrase, minCharLength, maxWordsLength))
                    {
                        phraseList.Add(phrase);
                    }

                    sb.Clear();
                }
                else
                {
                    sb.Append(word).Append(' ');
                }
            }

            string p2 = sb.ToString().Trim();

            if (!string.IsNullOrWhiteSpace(p2)
                && RakeHelpers.IsAcceptable(p2, minCharLength, maxWordsLength))
            {
                phraseList.Add(p2);
            }

            sb.Clear();
        }

        return phraseList;
    }
}
