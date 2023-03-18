﻿namespace BookGen.CommandArguments;

internal sealed class MdTableArguments : ArgumentsBase
{
    [Switch("d", "delimiter")]
    public char Delimiter { get; set; }

    public MdTableArguments()
    {
        Delimiter = '\t';
    }
}
