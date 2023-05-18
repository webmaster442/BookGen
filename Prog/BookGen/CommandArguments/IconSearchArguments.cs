//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal class IconSearchArguments : SearchArgumentsBase
{
    [Switch("i8", "icons8")]
    public bool? Icons8 { get; set; }

    [Switch("svg", "svgrepo")]
    public bool? SvgRepo { get; set; }

    public bool All
    {
        get => (!Icons8.HasValue && !SvgRepo.HasValue)
            || (Icons8 == true && SvgRepo == true);
    }
}
