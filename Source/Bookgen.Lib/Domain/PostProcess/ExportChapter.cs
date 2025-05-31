using System.ComponentModel;

namespace Bookgen.Lib.Domain.PostProcess;

public sealed class ExportChapter
{
    [Description("Chapter title")]
    public required string Title { get; init; }
    [Description("Chapter sub title")]
    public required string SubTitle { get; init; }
    [Description("Chapter contents")]
    public required List<ChapterItem> Items { get; init; }

}
