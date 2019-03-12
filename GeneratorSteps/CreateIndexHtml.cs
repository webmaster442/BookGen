//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using NLog;
using System;

namespace BookGen.GeneratorSteps
{
    internal class CreateIndexHtml : ITemplatedStep
    {
        public GeneratorContent Content { get; set; }
        public Template Template { get; set; }

        public void RunStep(GeneratorSettings settings, ILogger log)
        {
            Console.WriteLine("Generating Index file...");
            log.Info("Creating index.html");
            var input = settings.SourceDirectory.Combine(settings.Configruation.Index);
            var output = settings.OutputDirectory.Combine("index.html");

            Content.Content = MarkdownUtils.Markdown2WebHTML(input.ReadFile());
            var html = Template.ProcessTemplate(Content);
            output.WriteFile(html);
        }
    }
}
