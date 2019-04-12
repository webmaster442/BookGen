//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Utilities;
using System.Text;
using System.Text.RegularExpressions;
using BookGen.Core;

namespace BookGen.GeneratorSteps
{
    internal class CreatePrintableHtml : IGeneratorStep
    {
        private StringBuilder _content;
        private int _index;
        private Regex _indexExpression;

        private const string NewPage = "<p style=\"page-break-before: always\"></p>\r\n";

        public CreatePrintableHtml()
        {
            _content = new StringBuilder();
            _index = 1;
            _indexExpression = new Regex(@"(\[\^\d+\])", RegexOptions.Compiled);
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating Printable html...");
            var output = settings.OutputDirectory.Combine("print.html");

            CreateHeader();

            StringBuilder buffer = new StringBuilder();

            foreach (var chapter in settings.TocContents.Chapters)
            {
                buffer.AppendFormat("<h1>{0}</h1>\r\n\r\n", chapter);
                foreach (var file in settings.TocContents.GetFilesForChapter(chapter))
                {
                    log.Detail("Processing file for print output: {0}", file);
                    var input = settings.SourceDirectory.Combine(file);

                    var inputContent = input.ReadFile();

                    inputContent = Reindex(inputContent);
                    buffer.AppendLine(inputContent);
                    buffer.AppendLine(NewPage);
                }
            }

            _content.Append(MarkdownUtils.Markdown2PrintHTML(buffer.ToString()));

            CreateFooter();

            output.WriteFile(_content.ToString());
        }

        private string Reindex(string inputContent)
        {
            int numMatches = _indexExpression.Matches(inputContent).Count;

            if (numMatches < 1)
                return inputContent;

            inputContent = _indexExpression.Replace(inputContent, "REG$0");

            Regex r = null;
            for (int i = 0; i<(numMatches / 2); i++)
            {
                string expression = $"(REG\\[\\^{i+1}\\])";
                r = new Regex(expression, RegexOptions.Compiled);
                if (r.IsMatch(inputContent))
                {
                    inputContent = r.Replace(inputContent, $"[^{_index}]");
                    ++_index;
                }
            }

            return inputContent;

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
