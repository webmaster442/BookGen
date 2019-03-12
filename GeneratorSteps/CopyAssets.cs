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
    internal class CopyAssets : IGeneratorStep
    {
        public void RunStep(GeneratorSettings settings, ILogger log)
        {
            var assets = settings.SourceDirectory.Combine(settings.Configruation.AssetsDir);

            if (assets.IsExisting)
            {
                Console.WriteLine("Copy template assets to output...");
                var targetdir = settings.OutputDirectory.Combine(assets.GetName());
                log.Info("Copy directory {0} to {1}", assets, targetdir);
                assets.CopyDirectory(targetdir);
                targetdir.ProtectDirectory();
            }
        }
    }
}
