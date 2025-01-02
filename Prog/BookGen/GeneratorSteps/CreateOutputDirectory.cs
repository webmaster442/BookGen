//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.GeneratorSteps;

internal sealed class CreateOutputDirectory : IGeneratorStep
{
    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        if (settings.OutputDirectory.IsExisting)
        {
            log.LogInformation("Clearing previous build contents...");
            CleanDirectory(settings.OutputDirectory, log);
        }
        else
        {
            log.LogInformation("Creating output directory...");
            settings.OutputDirectory.CreateDir(log);
        }
    }

    public static void CleanDirectory(FsPath outputDirectory, ILogger log)
    {
        var di = new DirectoryInfo(outputDirectory.ToString());
        if (!di.Exists)
        {
            log.LogWarning("Directory doesn't exist: {outputDirectory}", outputDirectory);
            return;
        }
        foreach (FileInfo? file in di.GetFiles())
        {
            log.LogDebug("Deleting: {file}", file);
            file.Delete();
        }

        foreach (DirectoryInfo? dir in di.GetDirectories())
        {
            log.LogDebug("Deleting: {dir}", dir);
            dir.Delete(true);
        }
    }
}
