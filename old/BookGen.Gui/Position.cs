//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.Gui;

[DebuggerDisplay("{X}, {Y}")]
public readonly record struct Position
{
    public required int X { get; init; }
    public required int Y { get; init; }
}
