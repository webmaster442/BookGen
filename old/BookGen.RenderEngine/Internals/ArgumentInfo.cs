//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.RenderEngine.Internals;

public sealed record class ArgumentInfo
{
    /// <summary>
    /// Argument name
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Argument optionality
    /// </summary>
    public required bool Optional { get; init; }

    /// <summary>
    /// Argument description
    /// </summary>
    public required string Description { get; init; }
}