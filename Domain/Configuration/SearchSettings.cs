// ----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace BookGen.Domain
{
    
    public class SearchSettings
    {
        [Description("Search Page title")]
        public string SearchPageTitle { get; set; }
        [Description("Search Page search text placeholder text")]
        public string SearchTextBoxText { get; set; }
        [Description("Search Page search button text")]
        public string SearchButtonText { get; set; }
        [Description("Text to display number of search results")]
        public string SearchResults { get; set; }
        [Description("Text to display, when there are no search results")]
        public string NoResults { get; set; }

        public static SearchSettings Default
        {
            get
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
}
