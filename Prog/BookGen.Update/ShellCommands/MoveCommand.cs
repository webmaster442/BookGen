//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;

namespace BookGen.Update.ShellCommands;

internal sealed class MoveCommand : IShellCommand
{
    public required string Source { get; init; }
    public required string Target { get; init; }

    public string ToBash() => $"mv \"{Source}\" \"{Target}\"";

    public string ToPowerShell() => $"Move-Item -Force \"{Source}\" \"{Target}\"";
}
