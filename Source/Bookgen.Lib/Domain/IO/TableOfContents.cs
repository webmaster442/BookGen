using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO;

public sealed class TableOfContents
{
    [JsonPropertyName("$schema")]
    public string Schema => "bookgen.toc.schema.json";

    [FileExists]
    [Description("First page of the book")]
    public string IndexFile { get; init; }

    [Required]
    [Description("List of chapters in the book")]
    public TocChapter[] Chapters { get; init; }

    public TableOfContents()
    {
        Chapters = Array.Empty<TocChapter>();
        IndexFile = FileNameConstants.IndexFile;
    }

    public IEnumerable<string> GetFiles()
    {
        foreach (var chapter in Chapters)
        {
            if (chapter.Files != null)
            {
                foreach (var file in chapter.Files)
                {
                    yield return file;
                }
            }
        }
    }
}
