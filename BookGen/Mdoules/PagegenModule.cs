//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGen.Mdoules
{
    internal class PagegenModule : ModuleBase
    {
        public PagegenModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "PageGen";

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            throw new NotImplementedException();
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(PagegenModule));
        }
    }
}
