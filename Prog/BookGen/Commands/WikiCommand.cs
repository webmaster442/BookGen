//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Commands;

[CommandName("wiki")]
internal class WikiCommand : Command
{
    public override int Execute(string[] context)
    {
        return UrlOpener.OpenUrl(Constants.WikiUrl) ? Constants.Succes : Constants.GeneralError;
    }
}
