//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Configuration
{
    public class Translations: Dictionary<string, string>
    {
        public const string SearchPageTitle = "Search_PageTitle";
        public const string SearchTextBoxText = "Search_TextBoxText";
        public const string SearchButtonText = "Search_ButtonText";
        public const string SearchResults = "Search_Results";
        public const string SearchNoResults = "Search_NoResults";


        public static Translations CreateDefault()
        {
            return new Translations()
            {
                { SearchButtonText, "Search" },
                { SearchNoResults, "No Results found" },
                { SearchResults, "Results" },
                { SearchPageTitle, "Search" },
                { SearchTextBoxText, "Type here to search" }
            };
        }
    }
}
