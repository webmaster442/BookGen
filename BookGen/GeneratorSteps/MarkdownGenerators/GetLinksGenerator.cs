//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Domain;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookGen.GeneratorSteps.MarkdownGenerators
{
    internal class GetLinksGenerator : IMarkdownGenerator
    {
        private readonly Regex _link;

        public GetLinksGenerator()
        {
            _link = new Regex(@"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", RegexOptions.Compiled);
        }

        public string RunStep(RuntimeSettings settings, ILog log)
        {
            ConcurrentBag<string> links = new ConcurrentBag<string>();

            StringBuilder results = new StringBuilder();

            foreach (var chapter in settings.TocContents.Chapters)
            {
                log.Info("Processing chapter: {0}", chapter);
                results.AppendFormat("## {0}\r\n\r\n", chapter);
                links.Clear();

                Parallel.ForEach(settings.TocContents.GetLinksForChapter(chapter), link =>
                {
                    var input = settings.SourceDirectory.Combine(link.Url);

                    var contents = input.ReadFile(log);

                    foreach (Match? match in _link.Matches(contents))
                    {
                        if (match != null)
                            links.Add(match.Value);
                    }

                });

                foreach (string link in links.Distinct().OrderBy(s => s))
                {
                    results.AppendLine(link);
                }
                results.AppendLine();
            }

            return results.ToString();
        }
    }
}
