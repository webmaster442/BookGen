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

namespace BookGen.GeneratorSteps
{
    internal class CreateIndexHtml : ITemplatedStep
    {
        public IContent Content { get; set; }
        public Template Template { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating Index file...");
            var input = settings.SourceDirectory.Combine(settings.Configruation.Index);
            var output = settings.OutputDirectory.Combine("index.html");

            Content.Content = MarkdownRenderers.Markdown2WebHTML(input.ReadFile(), settings);
            var html = Template.Render();
            output.WriteFile(html);
        }
    }
}
