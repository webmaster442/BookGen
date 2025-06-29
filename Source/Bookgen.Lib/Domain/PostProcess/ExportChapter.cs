using System.ComponentModel;

namespace Bookgen.Lib.Domain.PostProcess;

public sealed class ExportChapter
{
    [Description("Chapter title")]
    public required string Title { get; init; }
    public required List<ChapterItem> Items { get; init; }

}
