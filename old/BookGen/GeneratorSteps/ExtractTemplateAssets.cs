//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Resources;

namespace BookGen.GeneratorSteps;

public sealed class ExtractTemplateAssets : IGeneratorStep
{
    public (KnownFile file, string targetPath)[] Assets { get; set; }

    public ExtractTemplateAssets()
    {
        Assets = new (KnownFile file, string targetPath)[0];
    }

    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        if (Assets.Length < 1)
        {
            log.LogWarning("External template used, skipping asset extract");
            return;
        }

        foreach ((KnownFile file, string targetPath) in Assets)
        {
            string? target = settings.OutputDirectory.Combine(targetPath).ToString();

            ResourceHandler.ExtractKnownFile(file, target, log);
        }
    }
}
