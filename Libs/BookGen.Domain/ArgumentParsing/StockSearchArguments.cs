//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


namespace BookGen.Domain.ArgumentParsing
{
    public sealed class StockSearchArguments : ArgumentsBase
    {
        [Switch("pe", "pexels", false)]
        public bool? Pexels { get; set; }
        [Switch("un", "unsplash", false)]
        public bool? Unsplash { get; set; }
        [Switch("pi", "pixabay", false)]
        public bool? Pixabay { get; set; }
        [Switch("s", "search", true)]
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

        public override bool Validate()
        {
            return !string.IsNullOrWhiteSpace(SearchTerms);
        }
    }
}
