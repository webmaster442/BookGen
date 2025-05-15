using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO;

public sealed class TocChapter
{
    [NotNullOrWhiteSpace]
    [Required]
    [Description("Chapter Title")]
    public string Title { get; init; }

    [Description("Chapter sub title")]
    public string SubTitle { get; init; }

    [Description("List of files associated with chapter")]
    [FileExists]
    public string[] Files { get; init; }

    public TocChapter()
    {
        Title = string.Empty;
        SubTitle = string.Empty;
        Files = Array.Empty<string>();
    }
}
