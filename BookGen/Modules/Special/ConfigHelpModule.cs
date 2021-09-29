//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Utilities;
using System;

namespace BookGen.Modules.Special
{
    internal class ConfigHelpModule : ModuleBase
    {
        public override string ModuleCommand => "ConfigHelp";

        public override ModuleRunResult Execute(string[] arguments)
        {
            Console.WriteLine(HelpUtils.DocumentConfiguration());
            return ModuleRunResult.Succes;
        }
    }
}
