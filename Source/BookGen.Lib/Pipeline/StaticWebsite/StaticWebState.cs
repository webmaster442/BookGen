//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Concurrent;

using BookGen.Lib.Domain;

namespace BookGen.Lib.Pipeline.StaticWebsite;

internal sealed class StaticWebState
{
    public ConcurrentDictionary<string, SourceFile> SourceFiles { get; } = new();
    public string Toc { get; set; } = string.Empty;

    public List<string> TocLinks { get; } = new List<string>();
}
