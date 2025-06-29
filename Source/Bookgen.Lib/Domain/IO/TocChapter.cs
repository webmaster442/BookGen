using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO;

public sealed class TocChapter
{
    [NotNullOrWhiteSpace]
    [Required]
    [Description("Chapter Title")]
    [MinLength(1)]
    public string Title { get; init; }

    [Description("List of files associated with chapter")]
    [FileExists]
    [MinLength(1)]
    public string[] Files { get; init; }

    public TocChapter()
    {
        Title = string.Empty;
        Files = Array.Empty<string>();
    }
}
