//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using NLog;
using System;
using System.IO;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class CreateTOCForWebsite : IGeneratorContentFillStep
    {
        public GeneratorContent Content { get; set; }

        public void RunStep(GeneratorSettings settings, ILogger log)
        {
            Console.WriteLine("Generating Table of Contents...");
            log.Info("Creating table of contents");
            StringBuilder toc = new StringBuilder();
            foreach (var chapter in settings.TocContents.Chapters)
            {
                toc.Append("<details open=\"true\">");
                toc.AppendFormat("<summary>{0}</summary>", chapter);
                toc.Append("<ul>");
                foreach (var link in settings.TocContents.GetLinksForChapter(chapter))
                {
                    var file = Path.ChangeExtension(link.Link, ".html");
                    var fullpath = $"{settings.Configruation.HostName}{file}";
                    toc.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", fullpath, link.DisplayString);
                }
                toc.Append("</ul>");
                toc.Append("</details>");
            }
            Content.TableOfContents = toc.ToString();
        }
    }
}
