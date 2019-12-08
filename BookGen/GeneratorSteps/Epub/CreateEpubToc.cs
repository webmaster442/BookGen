//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Domain.Epub.Ncx;
using BookGen.Framework;
using System.Collections.Generic;
using System.Text;

namespace BookGen.GeneratorSteps.Epub
{
    internal class CreateEpubToc : ITemplatedStep
    {
        public Template Template { get; set; }
        public IContent Content { get; set; }

        private void GenerateTocNcx(RuntimeSettings settings, ILog log)
        {
            log.Info("Creating epub toc.ncx...");
            var output = settings.OutputDirectory.Combine("epubtemp\\OPS\\toc.ncx");
            Ncx toc = new Ncx
            {
                Version = "2005-1",
                Xmlns = "http://www.daisy.org/z3986/2005/ncx/",
                Lang = "en",
                Head = new Head
                {
                    Meta = new List<Meta>
                    {
                        new Meta
                        {
                            Name = "dtb:uid",
                            Content = ""
                        },
                    }
                },
                DocTitle = new DocTitle
                {
                    Text = settings.Configuration.Metadata.Title
                },
                NavMap = new NavMap
                {
                    NavPoint = new NavPoint
                    {
                        Id = "root",
                        NavPoints = FillNavPoints(settings),
                    }
                }
            };

            var namespaces = new List<(string prefix, string namespac)>
            {
                ("", "http://www.daisy.org/z3986/2005/ncx/"),
                ("ncx", "http://www.daisy.org/z3986/2005/ncx/")
            };

            output.SerializeXml(toc, log, namespaces);
        }

        private List<NavPoint> FillNavPoints(RuntimeSettings settings)
        {
            var navPoint = new List<NavPoint>();
            int filecounter = 1;
            foreach (var link in settings.TocContents.GetLinksForChapter())
            {
                navPoint.Add(new NavPoint
                {
                    Id = $"navpoint-{filecounter}",
                    NavLabel = new NavLabel
                    {
                        Text = link.DisplayString
                    },
                    Content = new Content
                    {
                        Src = $"page_{filecounter:D3}.html"
                    }

                });
                ++filecounter;
            }
            return navPoint;
        }

        private void GenerateHtmlToc(RuntimeSettings settings, ILog log)
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

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            GenerateTocNcx(settings, log);
            GenerateHtmlToc(settings, log);
        }
    }
}