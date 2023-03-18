//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.GeneratorSteps;

internal sealed class CreatePagesJS : IGeneratorStep
{
    public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
    {
        log.Info("Generating pages.js...");
        var pages = new List<string>();
        foreach (string? file in settings.TocContents.Files)
        {
            pages.Add(settings.Configuration.HostName + Path.ChangeExtension(file, ".html"));
        }
        FsPath target = settings.OutputDirectory.Combine("pages.js");

        string javaScript = JsonInliner.InlineJs("pages", pages);

        target.WriteFile(log, javaScript);
    }
}
