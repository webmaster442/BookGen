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
        private readonly (string content, string targetPath)[] _assets;

        public ExtractTemplateAssets(params (string content, string targetPath)[] assets)
        {
            _assets = assets;
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            foreach (var asset in _assets)
            {
                var output = settings.OutputDirectory.Combine(asset.targetPath);
                output.WriteFile(asset.content);
            }
        }
    }
}
