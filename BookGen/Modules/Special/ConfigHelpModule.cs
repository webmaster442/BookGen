//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Utilities;
using System;

namespace BookGen.Modules.Special
{
    internal class ConfigHelpModule : BaseModule
    {
        public override string ModuleCommand => "ConfigHelp";

        public override bool Execute(string[] arguments)
        {
            Console.WriteLine(HelpUtils.DocumentConfiguration());
            Environment.Exit(1);
            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(ConfigHelpModule));
        }
    }
}
