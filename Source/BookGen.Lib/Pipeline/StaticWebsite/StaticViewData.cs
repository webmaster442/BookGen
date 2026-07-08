//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Rendering.Templates;

namespace BookGen.Lib.Pipeline.StaticWebsite;

public sealed class StaticViewData : ViewData
{
    public required string Toc { get; init; }
}
