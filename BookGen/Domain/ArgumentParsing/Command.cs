//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace BookGen.Domain.ArgumentParsing
{
    internal enum Command
    {
        [Description("Build mode")]
        Build,
        [Description("Update to latest version")]
        Update,
        [Description("Command line gui")]
        Gui,
        [Description("Display config file information")]
        ConfigHelp,
    }
}
