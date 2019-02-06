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
    internal class CreateOutputDirectory : IGeneratorStep
    {
        public void RunStep(GeneratorSettings settings)
        {
            if (settings.OutputDirectory.IsExisting)
            {
                Console.WriteLine("Clearing previous build contents...");
                CleanDirectory(settings.OutputDirectory);
            }
            else
            {
                Console.WriteLine("Creating output directory...");
                settings.OutputDirectory.CreateDir();
            }
        }

        private void CleanDirectory(FsPath outputDirectory)
        {
            DirectoryInfo di = new DirectoryInfo(outputDirectory.ToString());
            foreach (var file in di.GetFiles())
                file.Delete();

            foreach (var dir in di.GetDirectories())
                dir.Delete(true);
        }
    }
}
