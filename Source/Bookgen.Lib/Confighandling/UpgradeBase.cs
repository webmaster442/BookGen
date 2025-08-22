//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Nodes;

namespace Bookgen.Lib.Confighandling;

internal abstract class UpgradeBase
{
    public abstract VersionTagInfo VersionTagInfo { get; }
    public abstract bool UpgradeToc(JsonObject tocFile);
    public abstract bool UpgradeConfig(JsonObject config);
}