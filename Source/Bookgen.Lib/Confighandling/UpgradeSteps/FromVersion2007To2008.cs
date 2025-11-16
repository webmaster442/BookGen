//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Nodes;

using Bookgen.Lib.Domain.IO.Configuration;

namespace Bookgen.Lib.Confighandling.UpgradeSteps;

internal sealed class FromVersion2007To2008 : UpgradeBase
{
    public override VersionTagInfo VersionTagInfo
        => new VersionTagInfo(2007, 2008);

    public override bool UpgradeConfig(JsonObject config)
    {
        config.Add("FeedConfig", JsonValue.Create(new FeedConfig()));
        return true;
    }

    public override bool UpgradeToc(JsonObject tocFile)
        => false;
}
