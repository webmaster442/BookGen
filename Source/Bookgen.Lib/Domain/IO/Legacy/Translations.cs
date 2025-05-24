//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Domain.IO.Legacy;

[Serializable]
public sealed class Translations : Dictionary<string, string>
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
    public const string NavigateNext = "Navigate_Next";
    public const string NavigatePrevious = "Navigate_Previous";
    public const string NavigateSearch = "Navigate_Search";
    public const string NavigateSubChapters = "Navigate_OnThisPage";
    public const string NavigateContents = "Navigate_Contents";
    public const string NavigateTop = "Navigate_Top";

    public Translations() : base()
    {
    }

    public Translations(IDictionary<string, string> dictionary) : base(dictionary)
    {
    }

    public Translations(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
    {
    }

    public Translations(IEnumerable<KeyValuePair<string, string>> collection) : base(collection)
    {
    }

    public Translations(IEnumerable<KeyValuePair<string, string>> collection, IEqualityComparer<string> comparer) : base(collection, comparer)
    {
    }

    public Translations(IEqualityComparer<string> comparer) : base(comparer)
    {
    }

    public Translations(int capacity) : base(capacity)
    {
    }

    public Translations(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
    {
    }

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
            { CookeAccept, "Okay" },
            { NavigateNext, "Next"},
            { NavigatePrevious, "Previous"},
            { NavigateSearch, "Search"},
            { NavigateSubChapters, "On this page"},
            { NavigateContents, "Contents"},
            { NavigateTop, "Go to top"},
        };
    }
}
