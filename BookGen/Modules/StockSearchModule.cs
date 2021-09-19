//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Utilities;
using System.Threading;

namespace BookGen.Modules
{
    internal class StockSearchModule : ModuleWithState
    {
        public StockSearchModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "StockSearch";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-s",
                                            "--search",
                                            "-pe",
                                            "--pexels",
                                            "-un",
                                            "--unsplash",
                                            "-pi",
                                            "--pixabay");
            }
        }

        public override bool Execute(string[] arguments)
        {
            var args = new StockSearchArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            if (args.Pixabay == true || args.All)
            {
                CurrentState.Log.Info("Searching on Pixabay...");
                PerformSearch(Constants.PixabaySearchUrl, CurrentState.Log, args.SearchTerms);
            }
            if (args.Pexels == true || args.All)
            {
                CurrentState.Log.Info("Searching on Pexels...");
                PerformSearch(Constants.PexelSearchUrl, CurrentState.Log, args.SearchTerms);
            }
            if (args.Unsplash == true || args.All)
            {
                CurrentState.Log.Info("Searching on Unsplash...");
                PerformSearch(Constants.UnsplashSearchUrl, CurrentState.Log, args.SearchTerms);
            }

            return true;

        }

        private void PerformSearch(string searchUrl, ILog log, string searchTerms)
        {
            if (!UrlOpener.OpenUrlWithParameters(searchUrl, searchTerms))
            {
                log.Warning("Coudn't open: {0}", searchUrl);
            }
            Thread.Sleep(100);
        }
    }
}
