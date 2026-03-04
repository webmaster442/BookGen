//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Nodes;

namespace Bookgen.Lib.Confighandling.UpgradeSteps;

internal sealed class FromVersion2004To2005 : UpgradeBase
{
    public override VersionTagInfo VersionTagInfo
        => new VersionTagInfo(2004, 2005);

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

        bool oldvalue = imgObject.TryGetPropertyValue("ResizeAndRecodeImagesToWebp", out JsonNode? webpValue)
            && webpValue is JsonValue webpJsonValue
            && webpJsonValue.GetValue<bool>();

        imgObject.Remove("ResizeAndRecodeImagesToWebp");

        if (oldvalue)
            imgObject.Add("ResizeAndRecodeImages", JsonValue.Create("AsWebp"));
        else
            imgObject.Add("ResizeAndRecodeImages", JsonValue.Create("Passtrough"));
    }

    public override bool UpgradeToc(JsonObject tocFile)
        => false;
}
