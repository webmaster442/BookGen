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
using System.Linq;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class CreatePrintableHtml : ITemplatedStep
    {
        private const string NewPage = "<p style=\"page-break-before: always\"></p>\r\n";

        public TemplateProcessor? Template { get; set; }
        public IContent? Content { get; set; }

        public CreatePrintableHtml()
        {
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating Printable html...");
            settings.CurrentTargetFile = settings.OutputDirectory.Combine("print.html");

            StringBuilder buffer = new StringBuilder();

            foreach (var chapter in settings.TocContents.Chapters)
            {
                log.Info("Processing: {0}...", chapter);
                buffer.AppendFormat("<h1>{0}</h1>\r\n\r\n", chapter);
                foreach (var file in settings.TocContents.GetLinksForChapter(chapter).Select(l => l.Url))
                {
                    log.Detail("Processing file for print output: {0}", file);
                    var input = settings.SourceDirectory.Combine(file);

                    var inputContent = input.ReadFile(log);

                    var rendered = MarkdownRenderers.Markdown2PrintHTML(inputContent, settings);

                    buffer.AppendLine(rendered);
                    buffer.AppendLine(NewPage);
                }
            }

            Content.Content = buffer.ToString();
            settings.CurrentTargetFile.WriteFile(log, Template.Render());
        }
    }
}
