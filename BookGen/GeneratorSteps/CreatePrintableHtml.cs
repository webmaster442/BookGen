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
using BookGen.Utilities;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class CreatePrintableHtml : ITemplatedStep
    {
        private int _index;

        private const string NewPage = "<p style=\"page-break-before: always\"></p>\r\n";

        public Template? Template { get; set; }
        public IContent? Content { get; set; }

        public CreatePrintableHtml()
        {
            _index = 1;
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating Printable html...");
            var output = settings.OutputDirectory.Combine("print.html");

            StringBuilder buffer = new StringBuilder();

            foreach (var chapter in settings.TocContents.Chapters)
            {
                buffer.AppendFormat("<h1>{0}</h1>\r\n\r\n", chapter);
                foreach (var file in settings.TocContents.GetFilesForChapter(chapter))
                {
                    log.Detail("Processing file for print output: {0}", file);
                    var input = settings.SourceDirectory.Combine(file);

                    var inputContent = input.ReadFile(log);

                    inputContent = MarkdownUtils.Reindex(inputContent, ref _index);
                    buffer.AppendLine(inputContent);
                    buffer.AppendLine(NewPage);
                }
            }

            Content.Content = MarkdownRenderers.Markdown2PrintHTML(buffer.ToString(), settings);
            output.WriteFile(log, Template.Render());
        }
    }
}
