//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Commands;

namespace BookGen;

public class BookGenArgumentBase : ArgumentsBase, IVerbosablityToggle
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
