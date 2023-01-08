//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;

namespace BookGen.Update.ShellCommands;

internal sealed class WaitCommand : IShellCommand
{
    public required int Seconds { get; init; }

    public string ToBash() => $"sleep {Seconds}";

    public string ToPowerShell() => $"Start-Sleep -Seconds {Seconds}";
}
