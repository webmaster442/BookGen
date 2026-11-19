//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using Microsoft.Extensions.Logging;

using static BookGen.Shell.Shared.Loging.ExceptionLogger;

namespace BookGen.Shell.Shared.Loging;

public sealed class ExceptionLogger : ILoggerProvider, IAppendLine
{
    public interface IAppendLine
    {
        void AppendLine(string? message);
        void AppendLine();
    }

    private readonly StringBuilder _exceptionData;
    private readonly string _folder;
    private readonly Lock _lock;

    public ExceptionLogger(string folder)
    {
        _exceptionData = new StringBuilder();
        _folder = folder;
        _lock = new Lock();

    }

    public ILogger CreateLogger(string categoryName)
    {
        return new ExceptionDataLogger(categoryName, this);
    }

    public void Dispose()
    {
        if (_exceptionData.Length > 0)
        {
            try
            {
                string filePath = Path.Combine(_folder, $"exceptions_{DateTime.Now:yyyyMMdd_HHmmss}.log");
                File.WriteAllText(filePath, _exceptionData.ToString());
            }
            catch
            {
                // Ignore any exceptions thrown while writing the log file
            }
        }
    }

    public void AppendLine(string? message)
    {
        lock (_lock)
        {
            _exceptionData.AppendLine(message);
        }
    }

    public void AppendLine()
    {
        lock (_lock)
        {
            _exceptionData.AppendLine();
        }
    }

    internal sealed class ExceptionDataLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IAppendLine _appendLine;

        public ExceptionDataLogger(string categoryName, IAppendLine appendLine)
        {
            _categoryName = categoryName;
            _appendLine = appendLine;
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel)
            => logLevel >= LogLevel.Error;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel) && exception != null)
            {
                _appendLine.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{_categoryName}] {exception.Message}");
                _appendLine.AppendLine(exception.Message);
                _appendLine.AppendLine(exception.StackTrace);
                _appendLine.AppendLine();
            }
        }
    }
}
