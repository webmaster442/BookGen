//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public sealed record class CommandRunnerSettings
{
    public required (int code, string message) UnknownCommandCodeAndMessage { get; init; }
    public required int BadParametersExitCode { get; init; }
    public required int PlatformNotSupportedExitCode { get; init; }
    public required int ExcptionExitCode { get; init; }
    public required bool EnableUtf8Output { get; init; }
    public required bool PrintHelpOnBadArgs { get; init; }

    public static readonly CommandRunnerSettings Default = new()
    {
        UnknownCommandCodeAndMessage = (-1, "Unknown command"),
        BadParametersExitCode = -2,
        PlatformNotSupportedExitCode = -3,
        ExcptionExitCode = -4,
        EnableUtf8Output = true,
        PrintHelpOnBadArgs = false,
    };
}
