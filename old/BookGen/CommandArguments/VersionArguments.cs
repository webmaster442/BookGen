//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal class VersionArguments : ArgumentsBase
{
    [Switch("bd", "builddate")]
    public bool BuildDate { get; set; }

    [Switch("api", "apiversion")]
    public bool ApiVersion { get; set; }

    public bool IsDefault => !BuildDate && !ApiVersion;
}
