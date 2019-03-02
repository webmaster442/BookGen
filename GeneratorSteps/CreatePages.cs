//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using System;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreatePages : ITemplatedStep
    {
        public GeneratorContent Content { get; set; }
        public Template Template { get; set; }

        public void RunStep(GeneratorSettings settings)
        {
            Console.WriteLine("Generating Sub Markdown Files...");
            foreach (var file in settings.TocContents.Files)
            {
                var input = settings.SourceDirectory.Combine(file);
                var output = settings.OutputDirectory.Combine(Path.ChangeExtension(file, ".html"));

                var inputContent = input.ReadFile();

                Content.Title = MarkdownUtils.GetTitle(inputContent);
                Content.Content = MarkdownUtils.Markdown2WebHTML(inputContent);
                var html = Template.ProcessTemplate(Content);
                output.WriteFile(html);
            }
        }
    }
}
