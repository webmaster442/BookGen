//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;

using System.Diagnostics;

namespace BookGen.Shell.Cdg;

[DebuggerDisplay("{DisplayString}")]
internal class SelectionItemBase
{
    public required string DisplayString { get; init; }
    public required string Icon { get; init; }
    public required Color Color { get; init; }
    public bool IsMenu { get; init; } = false;
}
