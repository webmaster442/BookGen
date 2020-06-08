//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Domain;
using BookGen.Utilities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.GeneratorSteps.MarkdownGenerators
{
    internal class ChapterSummarizer : IMarkdownGenerator
    {
        private readonly RakeKeywordExtractor _keywordExtractor;

        public ChapterSummarizer(IEnumerable<string> stopwords)
        {
            _keywordExtractor = new RakeKeywordExtractor(stopwords);
        }

        public string RunStep(RuntimeSettings settings, ILog log)
        {
            var terms = new ConcurrentBag<string>();
            var results = new StringBuilder();

            foreach (string? chapter in settings.TocContents.Chapters)
            {
                log.Info("Processing chapter: {0}", chapter);
                results.AppendFormat("## {0}\r\n\r\n", chapter);
                terms.Clear();

                Parallel.ForEach(settings.TocContents.GetLinksForChapter(chapter), link =>
                {
                    var input = settings.SourceDirectory.Combine(link.Url);

                    var contents = input.ReadFile(log);

                    var keywords = _keywordExtractor.GetKeywords(contents, 15);
                    
                    foreach (var keyword in keywords)
                    {
                        terms.Add(keyword);
                    }

                });

                foreach (var term in terms.Distinct().OrderBy(s => s))
                {
                    results.Append(term);
                    results.Append(", ");
                }
                //remove the last ,
                results.Remove(results.Length - 2, 1);
                results.AppendLine();
                results.AppendLine();

            }

            return results.ToString();
        }
    }
}
