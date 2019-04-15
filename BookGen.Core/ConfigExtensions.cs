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
