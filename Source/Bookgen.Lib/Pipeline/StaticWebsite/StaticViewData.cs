//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Templates;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

public sealed class StaticViewData : ViewData
{
    public required string Toc { get; init; }
}
