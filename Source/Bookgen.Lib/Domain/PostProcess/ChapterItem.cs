using System.ComponentModel;

namespace Bookgen.Lib.Domain.PostProcess;

public sealed class ChapterItem
{
    [Description("Chapter tags")]
    public required string[] Tags { get; init; }

    [Description("Chapter item title")]
    public required string Title { get; init; }

    [Description("Chapter content rendered as html")]
    public required string Html { get; init; }
}
