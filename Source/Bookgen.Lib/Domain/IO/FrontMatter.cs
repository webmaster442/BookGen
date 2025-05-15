using Bookgen.Lib.Domain.Validation;

using YamlDotNet.Serialization;

namespace Bookgen.Lib.Domain.IO;

public sealed class FrontMatter
{
    [YamlMember(Alias = "title")]
    [NotNullOrWhiteSpace]
    public required string Title { get; init; }

    [YamlMember(Alias = "tags")]
    [NotNullOrWhiteSpace]
    public required string Tags { get; init; }

    [YamlMember(Alias = "template")]
    public string? Template { get; init; }

    [YamlMember(Alias = "data")]
    public Dictionary<string, string>? Data { get; init; }

    [YamlIgnore]
    public string[] TagArray
        => Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
}
