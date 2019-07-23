//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Domain;

namespace BookGen.GeneratorSteps
{
    internal class CopyAssets : IGeneratorStep
    {
        private readonly BuildConfig _target;

        public CopyAssets(BuildConfig target)
        {
            _target = target;
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Processing assets...");

            foreach (var asset in _target.TemplateAssets)
            {
                FsPath source = settings.SourceDirectory.Combine(asset.Source);
                FsPath target = settings.OutputDirectory.Combine(asset.Target);

                if (source.IsExisting
                    && source.Extension != ".md")
                {
                    source.Copy(target, log);
                }
            }
        }
    }
}
