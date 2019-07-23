//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class CreateSubpageIndexes : ITemplatedStep
    {
        public IContent Content { get; set; }
        public Template Template { get; set; }
        public List<HtmlLink> Chapters { get; private set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating index files for sub content folders...");
            foreach (var file in settings.TocContents.Files)
            {
                var dir = Path.GetDirectoryName(file);
                var output = settings.OutputDirectory.Combine(dir).Combine("index.html");
                if (!output.IsExisting)
                {
                    var mdcontent = CreateContentLinks(settings, dir);

                    Content.Title = dir;
                    Content.Content = MarkdownRenderers.Markdown2WebHTML(mdcontent, settings);
                    Content.Metadata = "";
                    var html = Template.Render();
                    output.WriteFile(html);
                    log.Detail("Creating file: {0}", output);
                }
            }
        }

        private string CreateContentLinks(RuntimeSettings settings, string dir)
        {
            if (Chapters == null)
                Chapters = settings.TocContents.GetLinksForChapter().ToList();

            var links = from link in Chapters
                        where link.Link.Contains(dir)
                        select link;

            StringBuilder sb = new StringBuilder();

            foreach (var link in links)
            {
                var flink = link.Link.Replace(".md", ".html").Replace(dir, ".");
                sb.AppendFormat("## [{0}]({1})\r\n", link.DisplayString, flink);
            }

            return sb.ToString();
        }
    }
}
