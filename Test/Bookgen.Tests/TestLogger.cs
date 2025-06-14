using Microsoft.Extensions.Logging;

namespace Bookgen.Tests;

internal class TestLogger : ILogger
{
    public int Errors { get; private set; }
    public int Warnings { get; private set; }

    public int Total { get; private set; }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => null;

    public bool IsEnabled(LogLevel logLevel)
        => true;

    public void Reset()
    {
        Errors = 0;
        Warnings = 0;
        Total = 0;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (LogLevel.Error == logLevel)
        {
            Errors++;
        }
        else if (LogLevel.Warning == logLevel)
        {
            Warnings++;
        }
        Total++;
    }
}
