//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.CrashReporting;

public sealed record class LogEntry
{
    public required DateTimeOffset Timestamp { get; init; }
    public required string LogLevel { get; init; }
    public required string Message { get; init; }
    public required string Exception { get; init; }
}
