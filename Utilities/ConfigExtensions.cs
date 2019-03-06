//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using System;

namespace BookGen.Utilities
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

        public static bool ValidateConfig(this Config config)
        {
            if (!config.Template.ToPath().IsExisting)
                return PrintError("Missing template file");
            if (!config.ImageDir.ToPath().IsExisting)
                return PrintError("Images directory doesn't exist");
            if (!config.TOCFile.ToPath().IsExisting)
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
        }
    }
}
