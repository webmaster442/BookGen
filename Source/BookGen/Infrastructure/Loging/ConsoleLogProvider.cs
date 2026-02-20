//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Microsoft.Extensions.Logging;

using Spectre.Console;

namespace BookGen.Infrastructure.Loging;

public sealed class ConsoleLogProvider : ILoggerProvider
{
    private readonly Dictionary<string, ConsoleLogger> _loggers = new();

    public ConsoleLogProvider()
    {
        _loggers = new Dictionary<string, ConsoleLogger>();
    }

    public ILogger CreateLogger(string categoryName)
    {
        if (_loggers.TryGetValue(categoryName, out ConsoleLogger? logger))
        {
            return logger;
        }
        else
        {
            logger = new ConsoleLogger(categoryName);
            _loggers.Add(categoryName, logger);
            return logger;
        }
    }

    public void Dispose()
    {
        foreach (KeyValuePair<string, ConsoleLogger> logger in _loggers)
        {
            logger.Value.Dispose();
        }
    }

    internal sealed class ConsoleLogger : IDisposable, ILogger
    {
        private readonly string _name;
        private readonly List<string> _logBuffer;

        public ConsoleLogger(string categoryName)
        {
            _logBuffer = new List<string>();
            _name = categoryName;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            => new DumyLogScope();

        public void Dispose()
        {
            if (_logBuffer.Count > 0)
            {
                foreach (var log in _logBuffer)
                {
                    AnsiConsole.MarkupLine(log);
                }
                _logBuffer.Clear();
            }
        }

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            static string LevelToString(LogLevel logLevel)
            {
                return logLevel switch
                {
                    LogLevel.Trace => $"{DateTime.Now.ToShortTimeString()} [grey]TRACE[/]: ",
                    LogLevel.Debug => $"{DateTime.Now.ToShortTimeString()} [blue]DEBUG[/]: ",
                    LogLevel.Information => $"{DateTime.Now.ToShortTimeString()} [green]INFO[/]: ",
                    LogLevel.Warning => $"{DateTime.Now.ToShortTimeString()} [yellow]WARN[/]: ",
                    LogLevel.Error => $"{DateTime.Now.ToShortTimeString()} [red]ERROR[/]: ",
                    LogLevel.Critical => $"{DateTime.Now.ToShortTimeString()} [red bold]CRITICAL[/]: ",
                    LogLevel.None => string.Empty,
                    _ => throw new UnreachableException(),
                };
            }

            var line = $"{LevelToString(logLevel)} {formatter(state, exception).EscapeMarkup()}";
            AnsiConsole.MarkupLine(line);

        }
    }
}
