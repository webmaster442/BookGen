using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO;

public sealed class TocEntry
{
    [NotNullOrWhiteSpace]
    [Required]
    public string Title { get; init; }

    public string SubTitle { get; init; }

    [FileExists]
    public string[] Files { get; init; }

    public TocEntry[] Entries { get; init; }

    public TocEntry()
    {
        Title = string.Empty;
        SubTitle = string.Empty;
        Entries = Array.Empty<TocEntry>();
        Files = Array.Empty<string>();
    }
}
