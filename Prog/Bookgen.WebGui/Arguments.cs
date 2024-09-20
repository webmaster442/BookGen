//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen.WebGui;

internal sealed class Arguments : ArgumentsBase
{
    [Switch("d", "dir")]
    public string Directory { get; set; }

    public Arguments()
    {
        Directory = Environment.CurrentDirectory;
    }
}
