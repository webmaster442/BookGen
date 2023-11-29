//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using Webmaster442.HttpServerFramework;

namespace BookGen.Framework;

internal sealed class JsonLog : ILog
{
    private readonly List<LogEntry> _entries;
    private readonly JsonSerializerOptions _options;

    public event EventHandler<LogEventArgs>? OnLogWritten;

    public JsonLog()
    {
        _entries = new List<LogEntry>(20);
        _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        _options.Converters.Add(new JsonStringEnumConverter());
    }

    public void Flush()
    {
        string? result = JsonSerializer.Serialize(_entries, _options);
        Console.WriteLine(result);
    }

    public LogLevel LogLevel { get; set; }

    public void Log(LogLevel logLevel, string format, params object[] args)
    {
        _entries.Add(new LogEntry
        {
            Message = string.Format(format, args),
            LogLevel = logLevel,
            TimeStamp = DateTime.UtcNow,
        });
        OnLogWritten?.Invoke(this, new LogEventArgs(logLevel, string.Format(format, args)));
    }
}
