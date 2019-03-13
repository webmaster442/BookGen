//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Domain;
using BookGen.Utilities;
using System;

namespace BookGen.GeneratorSteps
{
    internal class CopyImagesDirectory : IGeneratorStep
    {
        public void RunStep(GeneratorSettings settings, ILog log)
        {
            log.Info("Copy images to output...");
            var targetdir = settings.OutputDirectory.Combine(settings.ImageDirectory.GetName());
            settings.ImageDirectory.CopyDirectory(targetdir);
            targetdir.ProtectDirectory();
        }
    }
}
