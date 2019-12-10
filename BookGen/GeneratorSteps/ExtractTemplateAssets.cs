//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;

namespace BookGen.GeneratorSteps
{
    public class ExtractTemplateAssets : IGeneratorStep
    {
        public (string content, string targetPath)[] Assets { get; set; }

        public ExtractTemplateAssets()
        {
            Assets = new (string content, string targetPath)[0];
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Assets == null || Assets.Length < 1)
            {
                log.Warning("External template used, skipping asset extract");
                return;
            }

            foreach (var (content, targetPath) in Assets)
            {
                var output = settings.OutputDirectory.Combine(targetPath);
                output.WriteFile(log, content);
            }
        }
    }
}
