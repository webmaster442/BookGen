//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace Cdg;
internal class CdgArguments : ArgumentsBase
{
    [Switch("h", "hidden")]
    public bool ShowHidden { get; set; }
}
