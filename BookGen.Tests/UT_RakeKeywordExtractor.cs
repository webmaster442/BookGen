//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//---------------------------------------------------------------------------

using BookGen.Utilities;
using NUnit.Framework;
using System;
using System.Linq;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_RakeKeywordExtractor
    {
        private readonly string[] StopWordsEnglish = new string[]
        {
            "i",
             "me",
             "my",
             "myself",
             "we",
             "our",
             "ours",
             "ourselves",
             "you",
             "your",
             "yours",
             "yourself",
             "yourselves",
             "he",
             "him",
             "his",
             "himself",
             "she",
             "her",
             "hers",
             "herself",
             "it",
             "its",
             "itself",
             "they",
             "them",
             "their",
             "theirs",
             "themselves",
             "what",
             "which",
             "who",
             "whom",
             "this",
             "that",
             "these",
             "those",
             "am",
             "is",
             "are",
             "was",
             "were",
             "be",
             "been",
             "being",
             "have",
             "has",
             "had",
             "having",
             "do",
             "does",
             "did",
             "doing",
             "a",
             "an",
             "the",
             "and",
             "but",
             "if",
             "or",
             "because",
             "as",
             "until",
             "while",
             "of",
             "at",
             "by",
             "for",
             "with",
             "about",
             "against",
             "between",
             "into",
             "through",
             "during",
             "before",
             "after",
             "above",
             "below",
             "to",
             "from",
             "up",
             "down",
             "in",
             "out",
             "on",
             "off",
             "over",
             "under",
             "again",
             "further",
             "then",
             "once",
             "here",
             "there",
             "when",
             "where",
             "why",
             "how",
             "all",
             "any",
             "both",
             "each",
             "few",
             "more",
             "most",
             "other",
             "some",
             "such",
             "no",
             "nor",
             "not",
             "only",
             "own",
             "same",
             "so",
             "than",
             "too",
             "very",
             "s",
             "t",
             "can",
             "will",
             "just",
             "don",
             "should",
             "now"
        };

        [Test]
        public void EnsureThat_RakeKeywordExtractor_Works()
        {
            RakeKeywordExtractor extractor = new RakeKeywordExtractor(StopWordsEnglish);
            var keywords = extractor.GetKeywords("RAKE is short for Rapid Automatic Keyword Extraction algorithm. " +
                                                 "It is a domain independent keyword extraction algorithm, which tries to " +
                                                 "determine key phrases in a body of text by analyzing the frequency of word " +
                                                 "appearance and its co-occurance with other words in the text.", 5);

            string[] expected = new string[] { "rake", "body", "short", "tries", "text" };

            foreach (var keyword in keywords)
            {
                if (!expected.Contains(keyword))
                {
                    Assert.Fail($"{keyword} not found");
                }
            }
        }
    }
}
