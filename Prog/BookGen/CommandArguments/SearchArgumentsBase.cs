//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal class SearchArgumentsBase : ArgumentsBase
{
    [Switch("s", "search")]
    public string SearchTerms { get; set; }

    public SearchArgumentsBase()
    {
        SearchTerms = string.Empty;
    }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrEmpty(SearchTerms))
            return ValidationResult.Error("search term can't be empty");

        return ValidationResult.Ok();
    }
}
