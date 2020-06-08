//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Utilities;
using System;

namespace BookGen.Mdoules
{
    internal class PagegenModule : ModuleBase
    {
        public PagegenModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "PageGen";

        public bool TryGetArguments(ArgumentParser arguments, out PageGenParameters parsed)
        {
            parsed = new PageGenParameters();

            bool pageTypeSpecified = Enum.TryParse(arguments.GetSwitchWithValue("p", "page"), true, out PageType parsedPageType);
            parsed.PageType = parsedPageType;

            var dir = arguments.GetSwitchWithValue("d", "dir");

            if (!string.IsNullOrEmpty(dir))
                parsed.WorkDir = dir;

            return pageTypeSpecified;
        }

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            if (!TryGetArguments(tokenizedArguments, out PageGenParameters parameters))
                return false;

            switch (parameters.PageType)
            {
                case PageType.ExternalLinks:
                    break;
                case PageType.Phrases:
                    break;
            }

            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(PagegenModule));
        }
    }
}
