namespace BookGen.Infrastructure;

internal sealed class FileLoggerProvider : ILoggerProvider
{
    internal sealed class DumyScope : IDisposable
    {
        public void Dispose()
        {
            //empty
        }
    }

    internal sealed class FileLogger : ILogger
    {
        private readonly string _name;
        private readonly string _filePath;
        private readonly LogLevel _minLevel;

        public FileLogger(string name)
        {
            _name = name;
            _filePath = Path.Combine(Environment.CurrentDirectory, $"bookgen.log");
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return new DumyScope();
        }

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = $"{DateTime.Now} [{logLevel}] {_name}: {formatter(state, exception)}{Environment.NewLine}";

            File.AppendAllText(_filePath, message);
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(categoryName);
    }

    public void Dispose()
    {
        //empty
    }
}