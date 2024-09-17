﻿using System.Diagnostics;

namespace BookGen.Infrastructure;

internal sealed class DebugLoggerProvider : ILoggerProvider
{
    internal sealed class DumyScope : IDisposable
    {
        public void Dispose()
        {
            //empty
        }
    }

    internal sealed class DebugLogger : ILogger
    {
        private readonly string _name;

        public DebugLogger(string categoryName)
        {
            _name = categoryName;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return new DumyScope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = $"{DateTime.Now} [{logLevel}] {_name}: {formatter(state, exception)}{Environment.NewLine}";
            Debug.Write(message);
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DebugLogger(categoryName);
    }

    public void Dispose()
    {
        //empty
    }
}