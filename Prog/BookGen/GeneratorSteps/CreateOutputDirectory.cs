//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreateOutputDirectory : IGeneratorStep
    {
        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            if (settings.OutputDirectory.IsExisting)
            {
                log.Info("Clearing previous build contents...");
                CleanDirectory(settings.OutputDirectory, log);
            }
            else
            {
                log.Info("Creating output directory...");
                settings.OutputDirectory.CreateDir(log);
            }
        }

        public static void CleanDirectory(FsPath outputDirectory, ILog log)
        {
            var di = new DirectoryInfo(outputDirectory.ToString());
            if (!di.Exists)
            {
                log.Warning("Directory doesn't exist: {0}", outputDirectory);
                return;
            }
            foreach (FileInfo? file in di.GetFiles())
            {
                log.Detail("Deleting: {0}", file);
                file.Delete();
            }

            foreach (DirectoryInfo? dir in di.GetDirectories())
            {
                log.Detail("Deleting: {0}", dir);
                dir.Delete(true);
            }
        }
    }
}
