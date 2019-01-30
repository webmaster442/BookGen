//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using System;

namespace BookGen
{
    public class ConfigValidator
    {
        private static bool PrintError(string error)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: {0}", error);
            Console.ForegroundColor = color;
            return false;
        }

        public static bool ValidateConfig(Config config)
        {
            if (!config.ImageDir.ToPath().IsExisting)
                return PrintError("Images directory doesn't exist");
            if (!config.TOCFile.ToPath().IsExisting)
                return PrintError("TOC file doesn't exit");

            return true;
        }
    }
}
