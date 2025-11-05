//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Spectre.Console;

namespace BookGen.Shellprog.CommandCode.Cdg;

[DebuggerDisplay("{DisplayString}")]
internal class SelectionItemBase
{
    public string Id { get; init; } = string.Empty;
    public required string DisplayString { get; set; }
    public required string Icon { get; init; }
    public required Color Color { get; init; }
    public bool IsMenuHeader { get; init; } = false;
}
