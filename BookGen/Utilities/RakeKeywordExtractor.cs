//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Utilities
{
    internal class RakeKeywordExtractor
    {
        private readonly Regex _stopWordRegex;
        private const int minCharLength = 3;
        private const int maxWordsLength = 3;
        private const double minKeywordFrequency = 1;

        private static readonly Regex _sentenceDelimiters = new Regex(@"[\[\]\n.!?,;:\`\t\<\>\*\|\#\^\/\-\""\(\)\\\'\u2019\u2013]", RegexOptions.Compiled);
        private static readonly Regex _wordSplitter = new Regex(@"[^a-zA-Z0-9_\+\-/]", RegexOptions.Compiled);

        public string RegexPattern { get; private set; }

        public RakeKeywordExtractor(IEnumerable<string> stopWords)
        {
            StringBuilder regexPattern = new StringBuilder();

            foreach (var word in stopWords)
            {
                regexPattern.AppendFormat("\\b{0}\\b|", word);
            }

            //remove last pipe
            regexPattern.Remove(regexPattern.Length - 1, 1);

            RegexPattern = regexPattern.ToString();
            _stopWordRegex = new Regex(RegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        private IEnumerable<string> GenerateCandidateKeywords(IEnumerable<string> sentences)
        {
            foreach (var sentence in sentences)
            {
                var phrases = _stopWordRegex.Split(sentence.ToLowerInvariant());
                foreach (var phrase in phrases)
                {
                    if (!string.IsNullOrWhiteSpace(phrase)
                        && IsAcceptable(phrase, minCharLength, maxWordsLength))
                    {
                        yield return phrase.Trim();
                    }
                }
            }
        }

        private Dictionary<string, double> CalculateWordScores(IEnumerable<string> phraseList)
        {
            var wordFrequency = new Dictionary<string, double>();
            var wordDegree = new Dictionary<string, double>();

            foreach (string phrase in phraseList)
            {
                var words = SeparateWords(phrase, 0).ToList();

                int wordListDegree = words.Count - 1;

                foreach (string word in words)
                {
                    if (!wordFrequency.ContainsKey(word)) wordFrequency[word] = 0;

                    wordFrequency[word]++;

                    if (!wordDegree.ContainsKey(word)) wordDegree[word] = 0;

                    wordDegree[word] += wordListDegree;
                }
            }
            foreach (var item in wordFrequency)
            {
                wordDegree[item.Key] += wordFrequency[item.Key];
            }

            // Calculate Word scores = deg(w)/frew(w)
            var wordScore = new Dictionary<string, double>();
            foreach (var item in wordFrequency)
            {
                if (!wordScore.ContainsKey(item.Key)) wordScore[item.Key] = 0;

                wordScore[item.Key] = wordDegree[item.Key] / (wordFrequency[item.Key] * 1.0); // orig.
                                                                                              // word_score[item] = word_frequency[item]/(word_degree[item] * 1.0) #exp.
            }

            return wordScore;
        }

        private Dictionary<string, double> GenerateCandidateKeywordScores(IEnumerable<string> phraseList,
                                                                          Dictionary<string, double> wordScores)
        {
            var keywordCandidates = new Dictionary<string, double>();

            foreach (var phrase in phraseList)
            {
                /*if (minKeywordFrequency > 1
                    && phraseList.Count(s => s.Equals(phrase)) < minKeywordFrequency)
                {
                    continue;
                }*/

                if (!keywordCandidates.ContainsKey(phrase)) keywordCandidates[phrase] = 0;

                var words = SeparateWords(phrase, 0);
                keywordCandidates[phrase] = words.Sum(word => wordScores[word]);
            }

            return keywordCandidates;
        }


        public IEnumerable<string> GetKeywords(string input, int numberOfKeywords)
        {
            IEnumerable<string> sentences = SplitSentences(input);
            IEnumerable<string> candidates = GenerateCandidateKeywords(sentences);

            Dictionary<string, double> scores = CalculateWordScores(candidates);

            Dictionary<string, double> finalCandidates = GenerateCandidateKeywordScores(candidates, scores);

            return
                finalCandidates.OrderByDescending(x => x.Value)
                .Take(numberOfKeywords)
                .Select(x => x.Key);
        }

        private static IEnumerable<string> SplitSentences(string text)
        {
            return _sentenceDelimiters
                .Split(text)
                .Where(s => !string.IsNullOrEmpty(s));
        }

        private static bool IsAcceptable(string phrase, int minCharLength, int maxWordsLength)
        {
            if (phrase.Length < minCharLength) return false;

            string[] words = phrase.Split(' ');

            if (words.Length > maxWordsLength) return false;

            int digits = 0;
            int alpha = 0;

            for (int i = 0; i < phrase.Length; i++)
            {
                if (char.IsDigit(phrase[i])) digits++;
                if (char.IsLetter(phrase[i])) alpha++;
            }

            // a phrase must have at least one alpha character
            if (alpha == 0) return false;

            // a phrase must have more alpha than digits characters
            if (digits > alpha) return false;

            return true;
        }

        public static IEnumerable<string> SeparateWords(string phrase, int minWordReturnSize)
        {
            foreach (var singleWord in _wordSplitter.Split(phrase))
            {
                var currentWord = singleWord.Trim().ToLowerInvariant();
                // leave numbers in phrase, but don't count as words, since they tend to invalidate scores of their phrases

                if (!string.IsNullOrWhiteSpace(currentWord)
                    && currentWord.Length > minWordReturnSize
                    && !float.TryParse(currentWord, out _))
                {
                    yield return currentWord;
                }
            }
        }
    }
}
