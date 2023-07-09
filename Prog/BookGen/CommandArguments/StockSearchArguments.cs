//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class StockSearchArguments : SearchArgumentsBase
{
    [Switch("pe", "pexels")]
    public bool? Pexels { get; set; }
    [Switch("un", "unsplash")]
    public bool? Unsplash { get; set; }
    [Switch("pi", "pixabay")]
    public bool? Pixabay { get; set; }

    public bool All
    {
        get => (!Pexels.HasValue && !Unsplash.HasValue && !Pixabay.HasValue)
            || (Pexels == true && Unsplash == true && Pixabay == true);
    }
}
