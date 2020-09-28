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
using BookGen.Utilities;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

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

            var bag = new ConcurrentBag<(FsPath path, string content)>();


            Parallel.ForEach(settings.TocContents.Files,file =>
            {
                var input = settings.SourceDirectory.Combine(file);
                FsPath? target = settings.OutputDirectory.Combine(Path.ChangeExtension(file, ".html"));
                log.Detail("Processing file: {0}", input);

                var inputContent = input.ReadFile(log);

                Content.Title = MarkdownUtils.GetTitle(inputContent);

                if (string.IsNullOrEmpty(Content.Title))
                {
                    log.Warning("No title found in document: {0}", file);
                    Content.Title = file;
                }

                Content.Content = pipeline.RenderMarkdown(inputContent);
                Content.Metadata = settings.MetataCache[file];

                bag.Add((target, Template.Render()));

            });

            log.Info("Writing files to disk...");
            foreach (var item in bag)
            {
                item.path.WriteFile(log, item.content);
            }
        }
    }
}
