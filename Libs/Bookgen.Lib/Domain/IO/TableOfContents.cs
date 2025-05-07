using System.ComponentModel.DataAnnotations;

namespace Bookgen.Lib.Domain.IO;

public sealed class TableOfContents
{
    [Required]
    public TocChapter[] Chapters { get; init; }

    public TableOfContents()
    {
        Chapters = Array.Empty<TocChapter>();
    }
}
