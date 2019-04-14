//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Utilities;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class CreatePrintableHtml : IGeneratorStep
    {
        private StringBuilder _content;
        private int _index;

        private const string NewPage = "<p style=\"page-break-before: always\"></p>\r\n";

        public CreatePrintableHtml()
        {
            _content = new StringBuilder();
            _index = 1;
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating Printable html...");
            var output = settings.OutputDirectory.Combine("print.html");

            _content.AppendLine(Properties.Resources.html5header);

            StringBuilder buffer = new StringBuilder();

            foreach (var chapter in settings.TocContents.Chapters)
            {
                buffer.AppendFormat("<h1>{0}</h1>\r\n\r\n", chapter);
                foreach (var file in settings.TocContents.GetFilesForChapter(chapter))
                {
                    log.Detail("Processing file for print output: {0}", file);
                    var input = settings.SourceDirectory.Combine(file);

                    var inputContent = input.ReadFile();

                    inputContent = MarkdownUtils.Reindex(inputContent, ref _index);
                    buffer.AppendLine(inputContent);
                    buffer.AppendLine(NewPage);
                }
            }

            _content.Append(MarkdownRenderers.Markdown2PrintHTML(buffer.ToString(), settings.Configruation));

            _content.Append("</body></html>");

            output.WriteFile(_content.ToString());
        }
    }
}
