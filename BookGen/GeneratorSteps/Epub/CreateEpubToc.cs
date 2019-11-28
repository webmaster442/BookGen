//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Framework;
using System.Text;

namespace BookGen.GeneratorSteps.Epub
{
    internal class CreateEpubToc : ITemplatedStep
    {
        public Template Template { get; set; }
        public IContent Content { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating epub TOC...");

            StringBuilder buffer = new StringBuilder(4096);

            int index = 1;
            foreach (var chapter in settings.TocContents.Chapters)
            {
                buffer.AppendFormat("<h1>{0}</h1>\n", chapter);
                buffer.Append("<ol>");
                foreach (var link in settings.TocContents.GetLinksForChapter(chapter))
                {
                    buffer.AppendFormat("<li><a href=\"page_{0:D3}.html\">{1}</a></li>\n", index, link.DisplayString);
                    ++index;
                }
                buffer.Append("</ol>");
            }

            var output = settings.OutputDirectory.Combine($"epubtemp\\OPS\\nav.html");

            Template.Content = buffer.ToString();
            Template.Title = "";

            var html = Template.Render();
            output.WriteFile(log, html);
        }
    }
}