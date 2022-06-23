//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.DomainServices;
using BookGen.DomainServices.Markdown;
using BookGen.Framework;
using BookGen.Interfaces;

namespace BookGen.GeneratorSteps
{
    internal class CreatePrintableHtml : ITemplatedStep
    {
        private const string NewPage = "<p style=\"page-break-before: always\"></p>\r\n";

        public ITemplateProcessor? Template { get; set; }
        public IContent? Content { get; set; }

        public CreatePrintableHtml()
        {
        }

        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating Printable html...");
            FsPath? target = settings.OutputDirectory.Combine("print.html");

            using var pipeline = new BookGenPipeline(BookGenPipeline.Print);
            pipeline.InjectRuntimeConfig(settings);

            FootNoteReindexer reindexer = new(log, appendLineBreakbeforeDefs: true);

            foreach (var chapter in settings.TocContents.Chapters)
            {
                log.Info("Processing: {0}...", chapter);
                reindexer.AddMarkdown($"# {chapter}\r\n");
                foreach (var file in settings.TocContents.GetLinksForChapter(chapter).Select(l => l.Url))
                {
                    log.Detail("Processing file for print output: {0}", file);
                    var input = settings.SourceDirectory.Combine(file);

                    var inputContent = input.ReadFile(log, true);
                    reindexer.AddMarkdown(inputContent);
                }
            }

            Content.Content = pipeline.RenderMarkdown(reindexer.ToString());

            target.WriteFile(log, Template.Render());
        }
    }
}
