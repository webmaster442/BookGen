//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal class IconSearchArguments : SearchArgumentsBase
{
    
    public bool? Icons8 { get; set; }
    public bool? SvgRepo { get; set; }
    public bool? MaterialDesign { get; set; }

    public bool All
    {
        get => (!Icons8.HasValue && !SvgRepo.HasValue && !MaterialDesign.HasValue)
            || (Icons8 == true && SvgRepo == true && MaterialDesign == true);
    }
}
