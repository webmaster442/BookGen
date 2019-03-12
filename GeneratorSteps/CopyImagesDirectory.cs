//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using NLog;
using System;

namespace BookGen.GeneratorSteps
{
    internal class CopyImagesDirectory : IGeneratorStep
    {
        public void RunStep(GeneratorSettings settings, ILogger log)
        {
            Console.WriteLine("Copy images to output...");
            var targetdir = settings.OutputDirectory.Combine(settings.ImageDirectory.GetName());
            log.Info("copy directory {0} to {1}", settings.ImageDirectory, targetdir);
            settings.ImageDirectory.CopyDirectory(targetdir);
            targetdir.ProtectDirectory();
        }
    }
}
