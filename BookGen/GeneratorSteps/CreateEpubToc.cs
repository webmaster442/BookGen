//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Domain.Epub;
using System.Collections.Generic;

namespace BookGen.GeneratorSteps
{
    internal class CreateEpubToc : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Creating epub toc.ncx...");
            var output = settings.OutputDirectory.Combine("epubtemp\\OEBPS\\toc.ncx");
            Ncx toc = new Ncx
            {
                Version = "2005-1",
                Xmlns = "http://www.daisy.org/z3986/2005/ncx/",
                Head = new Head
                {
                    Meta = new List<MetaNcx>
                    {
                        new MetaNcx { Name = "dtb:totalPageCount", Content = "0" },
                        new MetaNcx { Name = "dtb:uid", Content = "0" },
                    }
                },
                DocTitle = new DocTitle
                {
                    Text = settings.Configruation.Metadata.Title
                },
                DocAuthor = new DocAuthor
                {
                    Text = settings.Configruation.Metadata.Author
                },
                NavMap = new NavMap
                {
                    NavPoint = new List<NavPoint>()
                }
            };
            FillNavPoints(toc.NavMap.NavPoint, settings);
            output.SerializeXml(toc);
        }

        private void FillNavPoints(List<NavPoint> navPoint, RuntimeSettings settings)
        {
            int chaptercounter = 1;
            foreach (var chapter in settings.TocContents.Chapters)
            {
                navPoint.Add(new NavPoint
                {
                    Id = $"navpoint-{chaptercounter}",
                    PlayOrder = chaptercounter.ToString(),
                    NavLabel = new NavLabel
                    {
                        Text = chapter
                    },
                    Content = new Content
                    {
                        Src = $"chapter_{chaptercounter:D2}.html"
                    }
                    
                });
                ++chaptercounter;
            }
        }
    }
}
