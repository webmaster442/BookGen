//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Framework;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreateSubpageIndexes: ITemplatedStep
    {
        public GeneratorContent Content { get; set; }
        public Template Template { get; set; }

        public void RunStep(GeneratorSettings settings, ILog log)
        {
            log.Info("Generating index files for sub content folders...");
            foreach (var file in settings.TocContents.Files)
            {
                var dir = Path.GetDirectoryName(file);
                var output = settings.OutputDirectory.Combine(dir).Combine("index.html");
                if (!output.IsExisting)
                {
                    Content.Title = dir;
                    Content.Content = "";
                    var html = Template.ProcessTemplate(Content);
                    output.WriteFile(html);
                    log.Detail("Creating file: {0}", output);
                }
            }
        }
    }
}
