//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using System;
using System.IO;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class GenerateSearchPage : ITemplatedStep
    {
        public Template Template { get; set; }
        public GeneratorContent Content { get; set; }

        private StringBuilder _buffer;

        public GenerateSearchPage()
        {
            _buffer = new StringBuilder();
        }

        public void RunStep(GeneratorSettings settings)
        {
            Console.WriteLine("Generating search page...");
            GenerateSearchForm(settings);
            GenerateSearchContents(settings);

            var output = settings.OutputDirectory.Combine("search.html");
            Content.Content = _buffer.ToString();

            var html = Template.ProcessTemplate(Content);
            output.WriteFile(html);
        }

        private void GenerateSearchForm(GeneratorSettings settings)
        {
            var options = settings.Configruation.SearchOptions;

            _buffer.Append("<div class=\"form-group\">");
            _buffer.AppendFormat("<h2>{0}</h2>", options.SearchPageTitle);
            _buffer.AppendFormat("<input type=\"text\" class=\"form-control\" id=\"searchtext\" placeholder=\"{0}\">", options.SearchTextBoxText);
            _buffer.AppendFormat("<button type=\"submit\" class=\"btn btn-default\">{0}</button>", options.SearchButtonText);
            _buffer.Append("</div>");
        }

        private void GenerateSearchContents(GeneratorSettings settings)
        {
            _buffer.Append("<div id=\"searchcontents\" style=\"visibility: collapse;\">");
            foreach (var chapter in settings.TocContents.Chapters)
            {
                foreach (var link in settings.TocContents.GetLinksForChapter(chapter))
                {
                    var fileContent = link.Link.ToPath().ReadFile();
                    var rendered = MarkdownUtils.Markdown2Plain(fileContent);

                    var file = Path.ChangeExtension(link.Link, ".html");
                    var fullpath = $"{settings.Configruation.HostName}{file}";

                    _buffer.AppendFormat("<div title=\"{0}\" data-link=\"{1}\">", link.DisplayString, fullpath);
                    _buffer.Append(rendered.Trim());
                    _buffer.Append("</div>");
                }
            }
            _buffer.Append("</div>");
        }
    }
}
