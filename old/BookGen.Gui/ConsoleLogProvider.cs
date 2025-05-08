//-----------------------------------------------------------------------------
// (c) 2024-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Cli.Mediator;

using Microsoft.Extensions.Logging;

using Spectre.Console;

using static BookGen.Gui.MessageTypes;

namespace BookGen.Gui;

public sealed class ConsoleLogProvider : ILoggerProvider
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, ConsoleLogger> _loggers = new();

    public ConsoleLogProvider(IMediator mediator)
    {
        _loggers = new Dictionary<string, ConsoleLogger>();
        _mediator = mediator;
    }

    public ILogger CreateLogger(string categoryName)
    {
        if (_loggers.TryGetValue(categoryName, out ConsoleLogger? logger))
        {
            return logger;
        }
        else
        {
            logger = new ConsoleLogger(categoryName, _mediator);
            _loggers.Add(categoryName, logger);
            return logger;
        }
    }

    public void Dispose()
    {
        foreach (var logger in _loggers)
        {
            logger.Value.Dispose();
        }
    }

    internal sealed class ConsoleLogger : IDisposable, ILogger, INotifyable<BeginLogRedirectMessage>, INotifyable<EndLogRedirectMessage>
    {
        private readonly string _name;
        private readonly IMediator _mediator;
        private readonly List<string> _logBuffer;
        private bool _isRedirected;

        public ConsoleLogger(string categoryName, IMediator mediator)
        {
            _logBuffer = new List<string>();
            _name = categoryName;
            _mediator = mediator;
            mediator.Register(this);
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            => new DumyLogScope();

        public void Dispose()
        {
            _mediator.Unregister(this);
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
            if (_isRedirected)
                _logBuffer.Add(line);
            else
                AnsiConsole.MarkupLine(line);

        }

        void INotifyable<BeginLogRedirectMessage>.OnNotify(BeginLogRedirectMessage message)
        {
            _isRedirected = true;
        }

        void INotifyable<EndLogRedirectMessage>.OnNotify(EndLogRedirectMessage message)
        {
            _isRedirected = false;
            foreach (var log in _logBuffer)
            {
                AnsiConsole.MarkupLine(log);
            }
            _logBuffer.Clear();
        }
    }
}
