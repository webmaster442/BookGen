//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Configuration
{
    public class Translations : Dictionary<string, string>
    {
        public const string SearchPageTitle = "Search_PageTitle";
        public const string SearchTextBoxText = "Search_TextBoxText";
        public const string SearchButtonText = "Search_ButtonText";
        public const string SearchResults = "Search_Results";
        public const string SearchNoResults = "Search_NoResults";

        public const string CookieHeader = "Cookie_Header";
        public const string CookieDescription = "Cookie_Description";
        public const string CookieLink = "Cookie_Link";
        public const string CookieLearnMore = "Cookie_LearnMore";
        public const string CookeAccept = "Cookie_Accept";

        public static Translations CreateDefault()
        {
            return new Translations()
            {
                { SearchButtonText, "Search" },
                { SearchNoResults, "No Results found" },
                { SearchResults, "Results" },
                { SearchPageTitle, "Search" },
                { SearchTextBoxText, "Type here to search" },
                { CookieHeader, "Do you like Cookies?" },
                { CookieDescription, "This Site uses cookies." },
                { CookieLink, "https://cookiesandyou.com/" },
                { CookieLearnMore, "Learn more" },
                { CookeAccept, "Okay" }
            };
        }
    }
}
