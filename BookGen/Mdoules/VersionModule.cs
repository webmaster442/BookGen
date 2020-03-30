//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Utilities;
using System;

namespace BookGen.Mdoules
{
    internal class VersionModule : ModuleBase
    {
        public VersionModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Version";

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            Console.WriteLine("BookGen Build date: {0} Starting...", Program.CurrentState.BuildDate);
            Console.WriteLine("Config API version: {0}", Program.CurrentState.ProgramVersion);
            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(VersionModule));
        }
    }
}
