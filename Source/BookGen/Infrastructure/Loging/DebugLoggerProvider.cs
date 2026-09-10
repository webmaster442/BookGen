//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.CrashReporting;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Loging;

internal sealed class DebugLoggerProvider : ILoggerProvider
{
    private readonly RingBuffer<LogEntry> _logBuffer;
    private readonly DebugLogger _logger;

    public DebugLoggerProvider(int bufferSize)
    {
        _logBuffer = new RingBuffer<LogEntry>(bufferSize);
        _logger = new DebugLogger(_logBuffer);
    }

    public ILogger CreateLogger(string categoryName)
        => _logger;

    public void Dispose()
    {
        // No resources to dispose
    }

    internal IEnumerable<LogEntry>? GetEntries()
    {
        return _logBuffer.ToArray();
    }

    internal sealed class DebugLogger(RingBuffer<LogEntry> logBuffer) : ILogger
    {
        private readonly RingBuffer<LogEntry> _logBuffer = logBuffer;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logBuffer.Add(new LogEntry
            {
                Timestamp = DateTimeOffset.UtcNow,
                LogLevel = logLevel.ToString(),
                Exception = exception?.ToString() ?? string.Empty,
                Message = formatter(state, exception)
            });
        }
    }
}
