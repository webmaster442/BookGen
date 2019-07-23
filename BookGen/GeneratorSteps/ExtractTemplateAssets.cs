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

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            if (Assets == null)
            {
                log.Warning("External template used, skipping asset extract");
                return;
            }

            foreach (var (content, targetPath) in Assets)
            {
                var output = settings.OutputDirectory.Combine(targetPath);
                output.WriteFile(content);
            }
        }
    }
}
