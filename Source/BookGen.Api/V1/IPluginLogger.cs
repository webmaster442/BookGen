//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents a logger that can be used to log messages with different severity levels.
/// </summary>
public interface IPluginLogger
{
    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}".</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    void LogError(string? message, params object?[] args);
    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}".</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    void LogCritical(Exception? exception, string? message, params object?[] args);
    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}".</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    void LogCritical(string? message, params object?[] args);
    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}".</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    void LogDebug(string? message, params object?[] args);
    /// <summary>
    /// Formats and writes an information log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}".</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    void LogInformation(string? message, params object?[] args);
    /// <summary>
    /// Formats and writes a warning log message.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}".</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    void LogWarning(Exception? exception, string? message, params object?[] args);
    /// <summary>
    /// Formats and writes a warning log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: "User {User} logged in from {Address}".</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    void LogWarning(string? message, params object?[] args);

}
