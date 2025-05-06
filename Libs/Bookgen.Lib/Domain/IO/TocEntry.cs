using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO;

public sealed class TocEntry
{
    [NotNullOrWhiteSpace]
    public required string Title { get; init; }
    public string SubTitle { get; init; } = string.Empty;
    public required string[] Files { get; init; }
    public TocEntry[] Entries { get; init; } = Array.Empty<TocEntry>();
}
