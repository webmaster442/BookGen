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
    internal class CopyAssets : IGeneratorStep
    {
        public void RunStep(GeneratorSettings settings, ILog log)
        {
            var assets = settings.SourceDirectory.Combine(settings.Configruation.AssetsDir);

            if (assets.IsExisting)
            {
                log.Info("Copy template assets to output...");
                var targetdir = settings.OutputDirectory.Combine(assets.GetName());
                assets.CopyDirectory(targetdir);
                targetdir.ProtectDirectory();
            }
        }
    }
}
