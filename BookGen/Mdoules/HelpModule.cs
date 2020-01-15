//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Framework;
using System;

namespace BookGen.Mdoules
{
    internal class HelpModule : ModuleBase
    {
        public HelpModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Help";

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            Console.WriteLine(HelpTextCreator.GenerateHelpText());
#if DEBUG
            Program.ShowMessageBox("Press a key to continue");
#endif
            Environment.Exit(1);

            return true;
        }
    }
}
