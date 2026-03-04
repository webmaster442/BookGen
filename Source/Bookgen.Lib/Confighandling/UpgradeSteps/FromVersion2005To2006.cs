//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Nodes;

namespace Bookgen.Lib.Confighandling.UpgradeSteps;

internal sealed class FromVersion2005To2006 : UpgradeBase
{
    public override VersionTagInfo VersionTagInfo
        => new VersionTagInfo(2005, 2006);

    public override bool UpgradeConfig(JsonObject config)
    {
        UpdateImgeObject(config.GetSubObjectOrThrow("StaticWebsiteConfig"));
        UpdateImgeObject(config.GetSubObjectOrThrow("WordpressConfig"));
        UpdateImgeObject(config.GetSubObjectOrThrow("PrintConfig"));
        return true;
    }

    public void UpdateImgeObject(JsonObject buildConfig)
    {
        JsonObject imgObject = buildConfig.GetSubObjectOrThrow("Images");

        int oldvalue = imgObject.TryGetPropertyValue("WebpQuality", out JsonNode? qualityValue)
            && qualityValue is JsonValue qualityJsonValue
            ? qualityJsonValue.GetValue<int>()
            : throw new InvalidOperationException("WebpQuality property not found or is not an integer.");

        imgObject.Remove("WebpQuality");
        imgObject.Add("ImageQualityOnResize", JsonValue.Create(oldvalue));
    }

    public override bool UpgradeToc(JsonObject tocFile)
        => false;
}
