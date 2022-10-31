//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace BookGen.RakeEngine.Internals;

internal static class RakeHelpers
{

    private static readonly Regex splitter = new Regex(@"[^a-zA-Z0-9_\+\-/]", RegexOptions.Compiled);
    private static readonly Regex sentenceDelimiters = new(@"[\[\]\n.!?,;:\t\-\""”“\(\)\\\'\u2019\u2013]", RegexOptions.Compiled);

    internal static Dictionary<string, float> CalculateWordScores(IEnumerable<string> lowerCasedPhraseList)
    {
        var wordFrequency = new Dictionary<string, float>();
        var wordDegree = new Dictionary<string, float>();

        foreach (string phrase in lowerCasedPhraseList)
        {
            string[] words = SeparateWords(phrase, 0).ToArray();
            int wordsLength = words.Length;

            int wordListDegree = wordsLength - 1;
            // if word_list_degree > 3: word_list_degree = 3 #exp.

            foreach (string? word in words)
            {
                if (!wordFrequency.ContainsKey(word)) wordFrequency[word] = 0f;

                wordFrequency[word] = wordFrequency[word] + 1f;

                if (!wordDegree.ContainsKey(word)) wordDegree[word] = 0f;

                wordDegree[word] = wordDegree[word] + wordListDegree; // orig.
                                                                      // word_degree[word] += 1/(word_list_length*1.0) #exp.
            }
        }
        foreach (KeyValuePair<string, float> item in wordFrequency)
        {
            wordDegree[item.Key] = wordDegree[item.Key] + wordFrequency[item.Key];
        }

        // Calculate Word scores = deg(w)/frew(w)
        var wordScore = new Dictionary<string, float>();
        foreach (KeyValuePair<string, float> item in wordFrequency)
        {
            if (!wordScore.ContainsKey(item.Key)) wordScore[item.Key] = 0;

            wordScore[item.Key] = wordDegree[item.Key] / (wordFrequency[item.Key] * 1.0f); // orig.
                                                                                           // word_score[item] = word_frequency[item]/(word_degree[item] * 1.0) #exp.
        }

        return wordScore;
    }

    internal static Dictionary<string, float> GenerateCandidateKeywordScores(List<string> phraseList,
                                                                             Dictionary<string, float> wordScores,
                                                                             double minKeywordFrequency)
    {
        var keywordCandidates = new Dictionary<string, float>();

        foreach (string phrase in phraseList)
        {
            if (minKeywordFrequency > 1)
            {
                if (phraseList.Count(s => s.Equals(phrase)) < minKeywordFrequency)
                    continue;
            }

            if (!keywordCandidates.ContainsKey(phrase)) keywordCandidates[phrase] = 0;

            IEnumerable<string> words = SeparateWords(phrase, 0);
            float candidateScore = words.Sum(word => wordScores[word]);

            keywordCandidates[phrase] = candidateScore;
        }

        return keywordCandidates;
    }

    internal static bool IsAcceptable(string phrase, int minCharLength, int maxWordsLength)
    {
        if (phrase.Length < minCharLength) return false;

        var wordSplitter = new StringSplitter(phrase.AsSpan(), ' ');

        int wordCount = 0;

        while (wordSplitter.TryGetNext(out _))
        {
            wordCount++;
        }

        if (wordCount > maxWordsLength)
        {
            return false;
        }

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

    internal static bool IsNumber(string word) => float.TryParse(word, out _);

    internal static IEnumerable<string> SeparateWords(string phrase, int minWordReturnSize)
    {
        foreach (string singleWord in splitter.Split(phrase))
        {
            string currentWord = singleWord.Trim();
            // leave numbers in phrase, but don't count as words, since they tend to invalidate scores of their phrases

            if (!string.IsNullOrWhiteSpace(currentWord) && currentWord.Length > minWordReturnSize &&
                !IsNumber(currentWord))
            {

                yield return currentWord;
            }
        }
    }

    internal static string[] SplitSentences(string text)
    {
        string[] sentences = sentenceDelimiters.Split(text);
        return sentences;
    }
}