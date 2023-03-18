//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class MdTableArguments : ArgumentsBase
{
    [Switch("d", "delimiter")]
    public char Delimiter { get; set; }

    public MdTableArguments()
    {
        Delimiter = '\t';
    }
}
