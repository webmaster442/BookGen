//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Domain.Rss;

namespace BookGen.GeneratorSteps;

internal sealed class CopyAssets : IGeneratorStep
{
    private readonly BuildConfig _target;

    public CopyAssets(BuildConfig target)
    {
        _target = target;
    }

    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        log.LogInformation("Processing assets...");

        foreach (Asset? asset in _target.TemplateAssets)
        {
            if (string.IsNullOrEmpty(asset.Source) || string.IsNullOrEmpty(asset.Target))
            {
                log.LogWarning("Skipping Asset, because no source or target defined");
                continue;
            }

            FsPath source = settings.SourceDirectory.Combine(asset.Source);
            FsPath target = settings.OutputDirectory.Combine(asset.Target);

            if (source.Extension == ".md")
            {
                log.LogWarning("Skipping markdown file: {file}", source);
                continue;
            }

            if (source.IsExisting)
            {
                if (asset.Minify
                    && Minify.TryMinify(source, log, out string? minified))
                {
                    target.WriteFile(log, minified);
                }
                else
                {
                    source.Copy(target, log);
                }
            }
            else
            {
                log.LogWarning("Asset not found: {file}", source);
                continue;
            }
        }
    }
}
