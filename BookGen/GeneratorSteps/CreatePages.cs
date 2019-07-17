//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreatePages : ITemplatedStep
    {
        public GeneratorContent Content { get; set; }
        public Template Template { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating Sub Markdown Files...");
            foreach (var file in settings.TocContents.Files)
            {
                var input = settings.SourceDirectory.Combine(file);
                var output = settings.OutputDirectory.Combine(Path.ChangeExtension(file, ".html"));
                log.Detail("Processing file: {0}", input);

                var inputContent = input.ReadFile();

                Content.Title = MarkdownUtils.GetTitle(inputContent);
                Content.Content = MarkdownRenderers.Markdown2WebHTML(inputContent, settings);
                Content.Metadata = settings.MetataCache[file];

                var html = Template.ProcessTemplate(Content);
                output.WriteFile(html);
            }
        }
    }
}
