//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
