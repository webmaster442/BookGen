//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

public class BookGenArgumentBase : ArgumentsBase
{
    [Switch("v", "verbose")]
    public bool Verbose { get; set; }

    [Switch("d", "dir")]
    public string Directory { get; set; }

    public BookGenArgumentBase()
    {
        Directory = Environment.CurrentDirectory;
    }
}
