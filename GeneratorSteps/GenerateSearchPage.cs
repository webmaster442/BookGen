﻿//-----------------------------------------------------------------------------
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
            GenerateSearchContents(settings);
            GenerateSearchForm(settings);

            var output = settings.OutputDirectory.Combine("search.html");
            Content.Title = settings.Configruation.SearchOptions.SearchPageTitle;
            Content.Content = _buffer.ToString();

            var html = Template.ProcessTemplate(Content);
            output.WriteFile(html);
        }

        private void GenerateSearchForm(GeneratorSettings settings)
        {
            var options = settings.Configruation.SearchOptions;
            var result = Properties.Resources.searchform.Replace("{0}", options.SearchPageTitle);
            result = result.Replace("{1}", options.SearchTextBoxText);
            result = result.Replace("{2}", options.SearchButtonText);
            result = result.Replace("{3}", options.SearchResults);
            _buffer.Append(result);
        }

        private void GenerateSearchContents(GeneratorSettings settings)
        {
            _buffer.Append("<div id=\"searchcontents\" style=\"display:none;\">");
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
