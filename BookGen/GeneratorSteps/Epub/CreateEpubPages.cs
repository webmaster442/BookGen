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
using BookGen.Resources;
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

        public TemplateProcessor? Template { get; set; }
        public IContent? Content { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating epub pages...");

            int index = 1;

            using var pipeline = new BookGenPipeline(BookGenPipeline.Epub);
            pipeline.InjectRuntimeConfig(settings);

            foreach (var file in settings.TocContents.Files)
            {
                _session.GeneratedFiles.Add($"page_{index:D3}");

                FsPath? target = settings.OutputDirectory.Combine($"epubtemp\\OPS\\page_{index:D3}.xhtml");


                log.Detail("Processing file for epub output: {0}", file);
                var input = settings.SourceDirectory.Combine(file);

                var inputContent = input.ReadFile(log);

                Content.Title = MarkdownUtils.GetTitle(inputContent);
                Content.Content = pipeline.RenderMarkdown(inputContent);

                var html = XhtmlNormalizer.Html5ToXhtml(Template.Render());

                target.WriteFile(log, html);
                ++index;
            }
        }
    }
}