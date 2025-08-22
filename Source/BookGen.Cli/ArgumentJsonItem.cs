//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public sealed class ArgumentJsonItem
{
    public required string Name { get; init; }
    public required string[] Arguments { get; init; }
}
