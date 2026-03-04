//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Nodes;

namespace Bookgen.Lib.Confighandling.UpgradeSteps;

internal sealed class FromVersion2003To2004 : UpgradeBase
{
    private const string TocProperty = "TocFile";
    private const string SubtitleProp = "SubTitle";

    public override VersionTagInfo VersionTagInfo
        => new VersionTagInfo(2003, 2004);

    public override bool UpgradeConfig(JsonObject config)
    {
        config.Remove(TocProperty);
        return true;
    }

    public override bool UpgradeToc(JsonObject tocFile)
    {
        JsonArray chaptersArray = tocFile.GetSubArrayOrThrow("Chapters");

        foreach (JsonNode? chapterNode in chaptersArray)
        {
            if (chapterNode is JsonObject chapter)
            {
                chapter.Remove(SubtitleProp);
            }
        }
        return true;
    }
}
