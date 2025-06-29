namespace Bookgen.Lib.Confighandling.Migration;

internal sealed class MigrationState
{
    public const string LegacyConfigFileName = "bookgen.json";
    public const string LegacyTagsFileName = "tags.json";
    public const string LegacyConfigFolder = ".bookgen";

    public Domain.IO.Legacy.Config LegacyConfig { get; set; } = new();

    public Dictionary<string, string[]> LegacyTags { get; set; } = new();

    public Domain.IO.Legacy.ToC LegacyToc { get; set; } = new();
}
