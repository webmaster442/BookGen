//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace Bookgen.Lib.Domain.PostProcess;

public sealed class ExportChapter
{
    [Description("Chapter title")]
    public required string Title { get; init; }
    public required List<ChapterItem> Items { get; init; }

}
