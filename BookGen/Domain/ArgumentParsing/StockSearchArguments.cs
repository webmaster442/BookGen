//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.ArgumentParser;

namespace BookGen.Domain.ArgumentParsing
{
    internal class StockSearchArguments : ArgumentsBase
    {
        [Switch("-pe", "--pexels")]
        public bool? Pexels { get; set; }
        [Switch("-un", "--unsplash")]
        public bool? Unsplash { get; set; }
        [Switch("-pi", "--pixabay")]
        public bool? Pixabay { get; set; }

        [Switch("-s", "--search")]
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
