//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Utilities;
using System;

namespace BookGen.Mdoules
{
    internal class ConfigHelpModule : ModuleBase
    {
        public ConfigHelpModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "ConfigHelp";

        public override bool Execute(ArgumentParser tokenizedArguments)
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
