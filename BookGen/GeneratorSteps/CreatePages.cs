//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreatePages : ITemplatedStep
    {
        public IContent? Content { get; set; }
        public TemplateProcessor? Template { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            if (Template == null)
                throw new DependencyException(nameof(Template));

            log.Info("Generating Sub Markdown Files...");

            using var pipeline = new BookGenPipeline(BookGenPipeline.Web);
            pipeline.InjectRuntimeConfig(settings);

            var bag = new ConcurrentBag<(string source, FsPath target, string title, string content)>();

            Parallel.ForEach(settings.TocContents.Files, file =>
            {

                (string source, FsPath target, string title, string content) result;

                var input = settings.SourceDirectory.Combine(file);
                result.target = settings.OutputDirectory.Combine(Path.ChangeExtension(file, ".html"));

                log.Detail("Processing file: {0}", input);

                var inputContent = input.ReadFile(log);

                result.title = MarkdownUtils.GetTitle(inputContent);

                if (string.IsNullOrEmpty(result.title))
                {
                    log.Warning("No title found in document: {0}", file);
                    result.title = file;
                }

                result.source = file;
                result.content = pipeline.RenderMarkdown(inputContent);

                bag.Add(result);

            });

            log.Info("Writing files to disk...");
            foreach (var item in bag)
            {
                Content.Title = item.title;
                Content.Metadata = settings.MetataCache[item.source];
                Content.Content = item.content;
                item.target.WriteFile(log, Template.Render());
            }
        }
    }
}
