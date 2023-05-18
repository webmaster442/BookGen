//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("stocksearch")]
internal class StockSearchCommand : Command<StockSearchArguments>
{
    private readonly ILog _log;

    public StockSearchCommand(ILog log)
    {
        _log = log;
    }

    public override int Execute(StockSearchArguments arguments, string[] context)
    {
        if (arguments.Pixabay == true || arguments.All)
        {
            _log.Info("Searching on Pixabay...");
            Search.Perform(Constants.PixabaySearchUrl, arguments.SearchTerms, _log);
        }
        if (arguments.Pexels == true || arguments.All)
        {
            _log.Info("Searching on Pexels...");
            Search.Perform(Constants.PexelSearchUrl, arguments.SearchTerms, _log);
        }
        if (arguments.Unsplash == true || arguments.All)
        {
            _log.Info("Searching on Unsplash...");
            Search.Perform(Constants.UnsplashSearchUrl, arguments.SearchTerms, _log);
        }

        return Constants.Succes;
    }

}
