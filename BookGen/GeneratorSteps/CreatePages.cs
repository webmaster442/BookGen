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
using System.IO;
#if RELEASE
using System.Threading.Tasks;
#endif

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
#if DEBUG
            foreach (var file in settings.TocContents.Files)
#endif
#if RELEASE
            Parallel.ForEach(settings.TocContents.Files, file =>
#endif
            {
                var input = settings.SourceDirectory.Combine(file);
                settings.CurrentTargetFile = settings.OutputDirectory.Combine(Path.ChangeExtension(file, ".html"));
                log.Detail("Processing file: {0}", input);

                var inputContent = input.ReadFile(log);

                Content.Title = MarkdownUtils.GetTitle(inputContent);

                if (string.IsNullOrEmpty(Content.Title))
                {
                    log.Warning("No title found in document: {0}", file);
                    Content.Title = file;
                }

                Content.Content = MarkdownRenderers.Markdown2WebHTML(inputContent, settings);
                Content.Metadata = settings.MetataCache[file];

                var html = Template.Render();
                settings.CurrentTargetFile.WriteFile(log, html);
            }
#if RELEASE
            );
#endif
        }
    }
}
