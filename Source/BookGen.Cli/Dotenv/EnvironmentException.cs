//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.Dotenv;

public sealed class EnvironmentException : Exception
{
    public static EnvironmentException Env001(string line)
        => new($"ENV001: Invalid Line Format: {line}");

    public static EnvironmentException Env002(string key)
        => new($"ENV002: Duplicate Key: {key}");

    public static EnvironmentException Env003(string line)
        => new($"ENV003: Invalid Key Format: {line}");

    public static EnvironmentException Env004()
        => new($"ENV004: Unclosed Quote");

    public static EnvironmentException Env005(string line)
        => new($"ENV005: Invalid Line Continuation: {line}");

    public static EnvironmentException Env006(string key)
        => new($"ENV006: Multi-line Key: {key}");

    public EnvironmentException(string message) : base(message)
    {
    }

    public EnvironmentException()
    {
    }

    public EnvironmentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
