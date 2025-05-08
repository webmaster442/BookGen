//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class ProjectConvertArguments : ArgumentsBase
{
    [Switch("b", "backup")]
    public bool Backup { get; set; }

    [Switch("d", "dir")]
    public string Directory { get; set; }

    public ProjectConvertArguments()
    {
        Directory = Environment.CurrentDirectory;
    }
}
