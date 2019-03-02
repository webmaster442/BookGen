//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using System;
using System.Text;

namespace BookGen.GeneratorSteps
{
    public class CreatePrintableHtml : IGeneratorStep
    {
        private StringBuilder _content;

        private const string NewPage = "<p style=\"page-break-before: always\"></p>\r\n";

        public CreatePrintableHtml()
        {
            _content = new StringBuilder();
        }

        public void RunStep(GeneratorSettings settings)
        {
            Console.WriteLine("Generating Printable html...");

            var output = settings.OutputDirectory.Combine("print.html");

            CreateHeader();

            foreach (var chapter in settings.TocContents.Chapters)
            {
                _content.AppendFormat("<h1>{0}</h1>\r\n", chapter);
                foreach (var file in settings.TocContents.GetFilesForChapter(chapter))
                {
                    var input = settings.SourceDirectory.Combine(file);

                    var inputContent = input.ReadFile();

                    var md = MarkdownUtils.Markdown2PrintHTML(inputContent);

                    _content.AppendLine(md);
                    _content.AppendLine(NewPage);
                }
            }

            CreateFooter();

            output.WriteFile(_content.ToString());
        }

        private void CreateFooter()
        {
            _content.Append("</body></html>");
        }

        private void CreateHeader()
        {
            _content.AppendLine("<!DOCTYPE html>");
            _content.AppendLine("<html lang=\"en\">");
            _content.AppendLine("<head><meta charset=\"utf-8\">");
            _content.AppendLine("</head><body>");
        }
    }
}
