//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
using BookGen.Domain.Configuration;
using BookGen.DomainServices;
using BookGen.Interfaces;

namespace BookGen.GeneratorSteps
{
    internal class CopyAssets : IGeneratorStep
    {
        private readonly BuildConfig _target;

        public CopyAssets(BuildConfig target)
        {
            _target = target;
        }

        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            log.Info("Processing assets...");

            foreach (var asset in _target.TemplateAssets)
            {
                if (string.IsNullOrEmpty(asset.Source) || string.IsNullOrEmpty(asset.Target))
                {
                    log.Warning("Skipping Asset, because no source or target defined");
                    continue;
                }

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
