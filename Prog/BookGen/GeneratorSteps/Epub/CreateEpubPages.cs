//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.DomainServices;
using BookGen.DomainServices.Markdown;
using BookGen.Framework;
using BookGen.Interfaces;
using BookGen.Resources;

namespace BookGen.GeneratorSteps.Epub
{
    internal class CreateEpubPages : ITemplatedStep
    {
        private readonly EpubSession _session;

        public CreateEpubPages(EpubSession session)
        {
            _session = session;
        }

        public ITemplateProcessor? Template { get; set; }
        public IContent? Content { get; set; }

        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
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

                Content.Title = MarkdownUtils.GetDocumentTitle(inputContent, log);
                Content.Content = pipeline.RenderMarkdown(inputContent);

                var html = XhtmlNormalizer.NormalizeToXHTML(Template.Render());

                target.WriteFile(log, html);
                ++index;
            }
        }
    }
}