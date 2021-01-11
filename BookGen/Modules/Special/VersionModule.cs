//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Utilities;
using System;

namespace BookGen.Modules.Special
{
    internal class VersionModule : ModuleBase
    {
        public override string ModuleCommand => "Version";

        public override bool Execute(string[] arguments)
        {
            Console.WriteLine("BookGen Build date: {0:yyyy:MM:dd}", Program.CurrentState.BuildDate.Date);
            Console.WriteLine("Config API version: {0}", Program.CurrentState.ProgramVersion);
            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(VersionModule));
        }
    }
}
