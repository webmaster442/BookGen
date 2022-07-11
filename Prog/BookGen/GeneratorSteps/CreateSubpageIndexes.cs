//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;
using BookGen.Framework;
using BookGen.Interfaces;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreateSubpageIndexes : ITemplatedStep
    {
        public IContent? Content { get; set; }
        public ITemplateProcessor? Template { get; set; }
        public List<Link>? Chapters { get; private set; }

        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating index files for sub content folders...");

            using var pipeline = new BookGenPipeline(BookGenPipeline.Web);
            pipeline.InjectRuntimeConfig(settings);

            foreach (string? file in settings.TocContents.Files)
            {
                string? dir = Path.GetDirectoryName(file);

                if (dir == null) continue;

                FsPath? target = settings.OutputDirectory.Combine(dir).Combine("index.html");
                if (!target.IsExisting)
                {
                    string? mdcontent = CreateContentLinks(settings, dir);

                    Content.Title = dir;
                    Content.Content = pipeline.RenderMarkdown(mdcontent);
                    Content.Metadata = "";
                    string? html = Template.Render();

                    log.Detail("Creating file: {0}", target);
                    target.WriteFile(log, html);
                }
            }
        }

        private string CreateContentLinks(IReadonlyRuntimeSettings settings, string dir)
        {
            if (Chapters == null)
                Chapters = settings.TocContents.GetLinksForChapter().ToList();

            IEnumerable<Link>? links = from link in Chapters
                                       where link.Url.Contains(dir)
                                       select link;

            var sb = new StringBuilder();

            foreach (Link? link in links)
            {
                string? flink = link.Url.Replace(".md", ".html").Replace(dir, ".");
                sb.AppendFormat("## [{0}]({1})\r\n", link.Text, flink);
            }

            return sb.ToString();
        }
    }
}
