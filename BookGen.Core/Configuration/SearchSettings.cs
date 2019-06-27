// ----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{

    public class SearchSettings
    {
        public string SearchPageTitle
        {
            get;
            set;
        }

        public string SearchTextBoxText
        {
            get;
            set;
        }

        public string SearchButtonText
        {
            get;
            set;
        }

        public string SearchResults
        {
            get;
            set;
        }

        public string NoResults
        {
            get;
            set;
        }

        public static SearchSettings CreateDefault()
        {
            return new SearchSettings
            {
                SearchPageTitle = "Search",
                SearchTextBoxText = "Type here to search",
                SearchButtonText = "Search",
                SearchResults = "Results",
                NoResults = "No Results found"
            };
        }
    }
}
