//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using System;

namespace BookGen.Core
{
    public static class ConfigExtensions
    {
        private static bool PrintError(string error)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: {0}", error);
            Console.ForegroundColor = color;
            return false;
        }

        public static bool ValidateConfig(this Config config, string workdir)
        {
            FsPath dir = new FsPath(workdir);

            if (!dir.Combine(config.Template).IsExisting)
                return PrintError("Missing template file");
            if (!dir.Combine(config.ImageDir).IsExisting)
                return PrintError("Images directory doesn't exist");
            if (!dir.Combine(config.TOCFile).IsExisting)
                return PrintError("TOC file doesn't exit");

            return true;
        }

        public static void UpgradeTo(this Config config, int targetVersion)
        {
            if (config.Version == 0 || config.Version < targetVersion)
                config.Version = targetVersion;

            if (config.StyleClasses == null)
                config.StyleClasses = new StyleClasses();

            if (config.SearchOptions == null)
                config.SearchOptions = SearchSettings.Default;

            if (config.Metadata == null)
                config.Metadata = Metadata.Default;

            if (config.PrecompileHeader == null)
                config.PrecompileHeader = Precompile.Default;
        }
    }
}
