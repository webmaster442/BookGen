using System.ComponentModel;

namespace Bookgen.Lib.Domain.PostProcess;

public sealed class PostProcessExport
{
    [Description("Book title")]
    public required string BookTitle { get; init; }

    [Description("Book chapters")]
    public required List<ExportChapter> Chapters { get; init; }
}
