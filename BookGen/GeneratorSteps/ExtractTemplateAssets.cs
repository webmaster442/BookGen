//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Domain;
using BookGen.Resources;
using System.IO;

namespace BookGen.GeneratorSteps
{
    public class ExtractTemplateAssets : IGeneratorStep
    {
        public (KnownFile file, string targetPath)[] Assets { get; set; }

        public ExtractTemplateAssets()
        {
            Assets = new (KnownFile file, string targetPath)[0];
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Assets.Length < 1)
            {
                log.Warning("External template used, skipping asset extract");
                return;
            }

            foreach (var (file, targetPath) in Assets)
            {
                var target = settings.OutputDirectory.Combine(targetPath).ToString();

                ResourceHandler.ExtractKnownFile(file, target, log);
            }
        }
    }
}
