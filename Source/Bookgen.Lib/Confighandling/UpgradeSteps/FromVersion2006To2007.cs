//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Nodes;

namespace Bookgen.Lib.Confighandling.UpgradeSteps;

internal sealed class FromVersion2006To2007 : UpgradeBase
{
    public override VersionTagInfo VersionTagInfo
        => new VersionTagInfo(2006, 2007);

    public override bool UpgradeConfig(JsonObject config)
    {
        config.Add("BookAuthor", JsonValue.Create(string.Empty));
        config.Add("Book2LetterISO639Language", JsonValue.Create("en"));
        return true;
    }
    public override bool UpgradeToc(JsonObject tocFile)
        => false;
}
