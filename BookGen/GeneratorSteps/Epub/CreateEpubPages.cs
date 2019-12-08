//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;

namespace BookGen.GeneratorSteps.Epub
{
    internal class CreateEpubPages : ITemplatedStep
    {
        private readonly EpubSession _session;

        public CreateEpubPages(EpubSession session)
        {
            _session = session;
        }

        public Template Template { get; set; }
        public IContent Content { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating epub pages...");

            int index = 1;
            foreach (var file in settings.TocContents.Files)
            {
                _session.GeneratedFiles.Add($"page_{index:D3}");

                var output = settings.OutputDirectory.Combine($"epubtemp\\OPS\\page_{index:D3}.xhtml");


                log.Detail("Processing file for epub output: {0}", file);
                var input = settings.SourceDirectory.Combine(file);

                var inputContent = input.ReadFile(log);

                Content.Title = MarkdownUtils.GetTitle(inputContent);
                Content.Content = MarkdownRenderers.Markdown2EpubHtml(inputContent, settings);

                var html = XhtmlNormalizer.NormalizeToXHTML(Template.Render());

                output.WriteFile(log, html);
                ++index;
            }
        }
    }
}