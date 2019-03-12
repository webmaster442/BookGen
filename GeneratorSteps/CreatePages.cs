//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using NLog;
using System;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreatePages : ITemplatedStep
    {
        public GeneratorContent Content { get; set; }
        public Template Template { get; set; }

        public void RunStep(GeneratorSettings settings, ILogger log)
        {
            Console.WriteLine("Generating Sub Markdown Files...");
            log.Info("processing markdown files found in table of contents");
            foreach (var file in settings.TocContents.Files)
            {
                var input = settings.SourceDirectory.Combine(file);
                var output = settings.OutputDirectory.Combine(Path.ChangeExtension(file, ".html"));
                log.Info("Processing file: {0}", input);

                var inputContent = input.ReadFile();

                Content.Title = MarkdownUtils.GetTitle(inputContent);
                Content.Content = MarkdownUtils.Markdown2WebHTML(inputContent);
                var html = Template.ProcessTemplate(Content);
                output.WriteFile(html);
            }
        }
    }
}
