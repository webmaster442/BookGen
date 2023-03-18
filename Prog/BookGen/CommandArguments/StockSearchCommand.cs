using BookGen.Cli;
using BookGen.Cli.Annotations;
using System.Threading;

namespace BookGen.CommandArguments
{
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
                PerformSearch(Constants.PixabaySearchUrl, arguments.SearchTerms);
            }
            if (arguments.Pexels == true || arguments.All)
            {
                _log.Info("Searching on Pexels...");
                PerformSearch(Constants.PexelSearchUrl, arguments.SearchTerms);
            }
            if (arguments.Unsplash == true || arguments.All)
            {
                _log.Info("Searching on Unsplash...");
                PerformSearch(Constants.UnsplashSearchUrl, arguments.SearchTerms);
            }

            return Constants.Succes;
        }

        private void PerformSearch(string searchUrl, string searchTerms)
        {
            if (!UrlOpener.OpenUrlWithParameters(searchUrl, searchTerms))
            {
                _log.Warning("Coudn't open: {0}", searchUrl);
            }
            Thread.Sleep(100);
        }

    }
}
