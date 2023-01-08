//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;

namespace BookGen.Update.ShellCommands;

internal sealed class EchoCommand : IShellCommand
{
    public required string Message { get; init; }

    public string ToBash() => $"echo \"{Message}\"";

    public string ToPowerShell() => $"Write-Host \"{Message}\"";
}