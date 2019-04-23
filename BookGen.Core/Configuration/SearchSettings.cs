// ----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{

    public class SearchSettings : ConfigurationBase
    {
        private string _SearchPageTitle;
        private string _SearchTextBoxText;
        private string _SearchButtonText;
        private string _SearchResults;
        private string _NoResults;

        public string SearchPageTitle
        {
            get => _SearchPageTitle;
            set => SetValue(ref _SearchPageTitle, value);
        }

        public string SearchTextBoxText
        {
            get => _SearchTextBoxText;
            set => SetValue(ref _SearchTextBoxText, value);
        }

        public string SearchButtonText
        {
            get => _SearchButtonText;
            set => SetValue(ref _SearchButtonText, value);
        }

        public string SearchResults
        {
            get => _SearchResults;
            set => SetValue(ref _SearchResults, value);
        }

        public string NoResults
        {
            get => _NoResults;
            set => SetValue(ref _NoResults, value);
        }

        public static SearchSettings CreateDefault()
        {
            return new SearchSettings
            {
                _SearchPageTitle = "Search",
                _SearchTextBoxText = "Type here to search",
                _SearchButtonText = "Search",
                _SearchResults = "Results",
                _NoResults = "No Results found"
            };
        }
    }
}
