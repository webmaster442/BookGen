//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
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
        public IContent? Content { get; set; }
        public TemplateProcessor? Template { get; set; }
        public List<Link>? Chapters { get; private set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating index files for sub content folders...");

            using var pipeline = new BookGenPipeline(BookGenPipeline.Web);
            pipeline.InjectRuntimeConfig(settings);

            foreach (var file in settings.TocContents.Files)
            {
                var dir = Path.GetDirectoryName(file);

                if (dir == null) continue;

                FsPath? target = settings.OutputDirectory.Combine(dir).Combine("index.html");
                if (!target.IsExisting)
                {
                    var mdcontent = CreateContentLinks(settings, dir);

                    Content.Title = dir;
                    Content.Content = pipeline.RenderMarkdown(mdcontent);
                    Content.Metadata = "";
                    var html = Template.Render();

                    log.Detail("Creating file: {0}", target);
                    target.WriteFile(log, html);
                }
            }
        }

        private string CreateContentLinks(RuntimeSettings settings, string dir)
        {
            if (Chapters == null)
                Chapters = settings.TocContents.GetLinksForChapter().ToList();

            var links = from link in Chapters
                        where link.Url.Contains(dir)
                        select link;

            StringBuilder sb = new StringBuilder();

            foreach (var link in links)
            {
                var flink = link.Url.Replace(".md", ".html").Replace(dir, ".");
                sb.AppendFormat("## [{0}]({1})\r\n", link.Text, flink);
            }

            return sb.ToString();
        }
    }
}
