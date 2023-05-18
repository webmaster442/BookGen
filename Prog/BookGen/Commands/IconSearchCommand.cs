//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("iconsearch")]
internal class IconSearchCommand : Command<IconSearchArguments>
{
    private readonly ILog _log;

    public IconSearchCommand(ILog log)
    {
        _log = log;
    }

    public override int Execute(IconSearchArguments arguments, string[] context)
    {
        if (arguments.Icons8 == true || arguments.All)
        {
            _log.Info("Searching on icons8...");
            Search.Perform(Constants.Icons8SearchUrl, arguments.SearchTerms, _log);
        }
        if (arguments.SvgRepo == true || arguments.All) 
        {
            _log.Info("Searching on svgrepo...");
            Search.Perform(Constants.SvgRepoSearchUrl, arguments.SearchTerms, _log);
        }
        return Constants.Succes;
    }
}
