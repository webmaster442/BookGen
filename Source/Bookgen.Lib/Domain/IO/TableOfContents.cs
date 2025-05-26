using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.IO;

public sealed class TableOfContents
{
    [JsonPropertyName("$schema")]
    public string Schema => "bookgen.toc.schema.json";

    [Required]
    public TocChapter[] Chapters { get; init; }

    public TableOfContents()
    {
        Chapters = Array.Empty<TocChapter>();
    }
}
