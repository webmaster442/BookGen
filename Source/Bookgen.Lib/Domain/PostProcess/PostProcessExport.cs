//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace Bookgen.Lib.Domain.PostProcess;

public sealed class PostProcessExport
{
    [Description("Book title")]
    public required string BookTitle { get; init; }

    [Description("Book chapters")]
    public required List<ExportChapter> Chapters { get; init; }
}
