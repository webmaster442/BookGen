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
        if (!tocFile.TryGetPropertyValue("Chapters", out var chapters)
            || chapters is not JsonArray chaptersArray)
        {
            throw new InvalidOperationException("Chapters property not found or is not an array.");
        }

        foreach (var chapterNode in chaptersArray)
        {
            if (chapterNode is JsonObject chapter)
            {
                chapter.Remove(SubtitleProp);
            }
        }
        return true;
    }
}
