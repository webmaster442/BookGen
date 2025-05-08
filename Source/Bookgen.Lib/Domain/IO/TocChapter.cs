using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO;

public sealed class TocChapter
{
    [NotNullOrWhiteSpace]
    [Required]
    public string Title { get; init; }

    public string SubTitle { get; init; }

    [FileExists]
    public string[] Files { get; init; }

    public TocChapter()
    {
        Title = string.Empty;
        SubTitle = string.Empty;
        Files = Array.Empty<string>();
    }
}
