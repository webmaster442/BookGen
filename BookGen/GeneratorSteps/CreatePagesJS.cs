//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Domain;
using System.Collections.Generic;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreatePagesJS : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating pages.js...");
            List<string> pages = new List<string>();
            foreach (var file in settings.TocContents.Files)
            {
                pages.Add(settings.Configuration.HostName + Path.ChangeExtension(file, ".html"));
            }
            FsPath target = settings.OutputDirectory.Combine("pages.js");

            string javaScript = JsonInliner.InlineJs("pages", pages, log);

            target.WriteFile(log, javaScript);
        }
    }
}
