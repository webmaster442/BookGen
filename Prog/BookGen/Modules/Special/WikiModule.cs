//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using BookGen.Framework;
using BookGen.Utilities;

namespace BookGen.Modules.Special
{
    internal class WikiModule : ModuleBase
    {
        public override string ModuleCommand => "Wiki";

        public override ModuleRunResult Execute(string[] arguments)
        {
            return UrlOpener.OpenUrl(Constants.WikiUrl) ? ModuleRunResult.Succes : ModuleRunResult.GeneralError;
        }

        public override string GetHelp()
        {
            return "Open the Wiki website";
        }
    }
}
