//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Rendering.Templates;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

public sealed class StaticViewData : ViewData
{
    public required string Toc { get; init; }
}
