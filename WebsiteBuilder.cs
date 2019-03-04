//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;
using System;
using System.IO;
using System.Text;

namespace BookGen
{
    internal class WebsiteBuilder : Generator
    {
        public WebsiteBuilder(Config configuration) : base(configuration)
        {
            FillToc();
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyAssets());
            AddStep(new GeneratorSteps.CopyImagesDirectory());
            AddStep(new GeneratorSteps.CreateIndexHtml());
            AddStep(new GeneratorSteps.CreatePagesJS());
            AddStep(new GeneratorSteps.CreatePages());
            AddStep(new GeneratorSteps.CreateSubpageIndexes());
        }

        private void FillToc()
        {
            Console.WriteLine("Generating Table of Contents...");
            StringBuilder toc = new StringBuilder();
            foreach (var chapter in Settings.TocContents.Chapters)
            {
                toc.Append("<details open=\"true\">");
                toc.AppendFormat("<summary>{0}</summary>", chapter);
                toc.Append("<ul>");
                foreach (var link in Settings.TocContents.GetLinksForChapter(chapter))
                {
                    var file = Path.ChangeExtension(link.Link, ".html");
                    var fullpath = $"{Settings.Configruation.HostName}{file}";
                    toc.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", fullpath, link.DisplayString);
                }
                toc.Append("</ul>");
                toc.Append("</details>");
            }
            GeneratorContent.TableOfContents = toc.ToString();
        }
    }
}
