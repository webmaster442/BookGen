﻿//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core.Configuration;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Resources;
using BookGen.Utilities;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class GenerateSearchPage : ITemplatedStep
    {
        public TemplateProcessor? Template { get; set; }
        public IContent? Content { get; set; }

        private readonly StringBuilder _buffer;
        private readonly Regex _spaces;

        public GenerateSearchPage()
        {
            _buffer = new StringBuilder();
            _spaces = new Regex(@"\s+", RegexOptions.Compiled);
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            Content.Metadata = FillMeta(settings.Configuration);

            log.Info("Generating search page...");
            GenerateSearchContents(settings, log);
            _buffer.Append(ResourceHandler.GetFile(KnownFile.SearchformHtml));

            FsPath? target = settings.OutputDirectory.Combine("search.html");
            Content.Title = settings.Configuration.Translations[Translations.SearchPageTitle];
            Content.Content = _buffer.ToString();

            var html = Template.Render();
            target.WriteFile(log, html);
        }

        private string FillMeta(Config configruation)
        {
            var meta = new MetaTag().FillWithConfigDefaults(configruation);
            meta.Title = $"{configruation.Metadata.Title} - {configruation.Translations[Translations.SearchPageTitle]}";
            meta.Description = configruation.Translations[Translations.SearchPageTitle];
            meta.Url = $"{configruation.HostName}search.html";
            return meta.GetHtmlMeta();
        }

        private void GenerateSearchContents(RuntimeSettings settings, ILog log)
        {
            _buffer.WriteElement(HtmlElement.Div, "searchcontents", "nodisplay");

            using var pipeline = new BookGenPipeline(BookGenPipeline.Plain);

            foreach (var chapter in settings.TocContents.Chapters)
            {
                foreach (var link in settings.TocContents.GetLinksForChapter(chapter))
                {
                    log.Detail("Processing file for search index: {0}", link.Url);
                    var fileContent = settings.SourceDirectory.Combine(link.Url).ReadFile(log);

                    var rendered = pipeline.RenderMarkdown(fileContent);

                    var file = Path.ChangeExtension(link.Url, ".html");
                    var fullpath = $"{settings.Configuration.HostName}{file}";

                    _buffer.AppendFormat("<div title=\"{0}\" data-link=\"{1}\">", link.Text, fullpath);
                    _buffer.Append(_spaces.Replace(rendered, " "));
                    _buffer.Append("</div>\n");
                }
            }
            _buffer.CloseElement(HtmlElement.Div);
        }
    }
}
