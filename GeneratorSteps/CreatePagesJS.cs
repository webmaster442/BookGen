//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreatePagesJS : IGeneratorStep
    {
        public void RunStep(GeneratorSettings settings)
        {
            Console.WriteLine("Generating pages.js...");
            List<string> pages = new List<string>(settings.TocFiles.Count);
            foreach (var file in settings.TocFiles)
            {
                pages.Add(settings.Configruation.HostName + Path.ChangeExtension(file, ".html"));
            }
            FsPath target = settings.OutputDirectory.Combine("pages.js");
            target.WriteFile("var pages=" + JsonConvert.SerializeObject(pages) + ";");
        }
    }
}
