namespace BookGen.CommandArguments;

internal sealed class StockSearchArguments : ArgumentsBase
{
    [Switch("pe", "pexels")]
    public bool? Pexels { get; set; }
    [Switch("un", "unsplash")]
    public bool? Unsplash { get; set; }
    [Switch("pi", "pixabay")]
    public bool? Pixabay { get; set; }
    [Switch("s", "search")]
    public string SearchTerms { get; set; }

    public bool All
    {
        get => (!Pexels.HasValue && !Unsplash.HasValue && !Pixabay.HasValue)
            || (Pexels == true && Unsplash == true && Pixabay == true);
    }

    public StockSearchArguments()
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
