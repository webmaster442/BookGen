﻿//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Domain;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreateOutputDirectory : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
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
            DirectoryInfo di = new DirectoryInfo(outputDirectory.ToString());
            if (!di.Exists)
            {
                log.Warning("Directory doesn't exist: {0}", outputDirectory);
                return;
            }
            foreach (var file in di.GetFiles())
            {
                log.Detail("Deleting: {0}", file);
                file.Delete();
            }

            foreach (var dir in di.GetDirectories())
            {
                log.Detail("Deleting: {0}", dir);
                dir.Delete(true);
            }
        }
    }
}
