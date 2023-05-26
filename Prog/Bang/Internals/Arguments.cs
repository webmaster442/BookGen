//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;

namespace Bang.Internals;

internal class Arguments : ArgumentsBase
{
    public string BangName { get; set; }
    public string SearchTerm { get; set; }

    public bool ListBangs { get; set; }
    public bool ShowUrls { get; set; }

    public Arguments()
    {
        BangName = string.Empty;
        SearchTerm = string.Empty;
    }

    public override ValidationResult Validate()
    {
        if (ShowUrls && ListBangs)
            return ValidationResult.Error("Listing bangs and showing bangs are incompatible options");

        if (ListBangs)
        {
            return !string.IsNullOrEmpty(SearchTerm)
                ? ValidationResult.Ok()
                : ValidationResult.Error("When listing bangs bang name can't be given");
        }

        ValidationResult validationResult = new();

        if (string.IsNullOrWhiteSpace(SearchTerm))
            validationResult.AddIssue("Search term can't be emtpy or whitespace");

        if (string.IsNullOrWhiteSpace(BangName))
            validationResult.AddIssue("Bang name can't be empty or whitespace");

        return validationResult;
    }
}
