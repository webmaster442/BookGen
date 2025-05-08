//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class StatArguments : ArgumentsBase
{
    [Switch("d", "dir")]
    public string Directory { get; set; }

    [Switch("i", "input")]
    public string Input { get; set; }

    public StatArguments()
    {
        Directory = Environment.CurrentDirectory;
        Input = string.Empty;
    }
}
