﻿//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.DomainServices;
using BookGen.DomainServices.Markdown;
using BookGen.Framework;
using BookGen.Interfaces;

namespace BookGen.GeneratorSteps
{
    internal class CreateIndexHtml : ITemplatedStep
    {
        public IContent? Content { get; set; }
        public ITemplateProcessor? Template { get; set; }

        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating Index file...");
            var input = settings.SourceDirectory.Combine(settings.Configuration.Index);
            FsPath? target = settings.OutputDirectory.Combine("index.html");

            using (var pipeline = new BookGenPipeline(BookGenPipeline.Web))
            {
                pipeline.InjectRuntimeConfig(settings);

                Content.Content = pipeline.RenderMarkdown(input.ReadFile(log));
            }

            var html = Template.Render();

            target.WriteFile(log, html);
        }
    }
}
