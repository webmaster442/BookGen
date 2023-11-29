//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api
{
    /// <summary>
    /// Interface for logging. Provides methods that allow information logging
    /// to console.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Write out all remaining info in buffered implementations.
        /// </summary>
        void Flush()
        {
            //by default do nothing
        }

        /// <summary>
        /// Current log level
        /// </summary>
        LogLevel LogLevel { get; set; }

        /// <summary>
        /// Raised when the log is written
        /// </summary>
        event EventHandler<LogEventArgs>? OnLogWritten;

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="format">Message, a fomat string that can be handled by the String.Format method</param>
        /// <param name="args">Arguments for formatting</param>
        void Log(LogLevel logLevel, string format, params object[] args);

        /// <summary>
        /// Log a Critical error.
        /// Critcal error is an error that causes the program to stop working
        /// </summary>
        /// <param name="format">Message, a fomat string that can be handled by the String.Format method</param>
        /// <param name="args">Arguments for formatting</param>
        void Critical(string format, params object[] args)
            => Log(LogLevel.Critical, format, args);

        /// <summary>
        /// Log a critical exception.
        /// </summary>
        /// <param name="ex">Exception to log</param>
        void Critical(Exception ex)
            => Log(LogLevel.Critical, "{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace ?? string.Empty);

        /// <summary>
        /// Log a warning message.
        /// Warning is an error that shouldn't happen idealy, but we are expecting it to occure.
        /// </summary>
        /// <param name="format">Message, a fomat string that can be handled by the String.Format method</param>
        /// <param name="args">Arguments for formatting</param>
        void Warning(string format, params object[] args)
            => Log(LogLevel.Warning, format, args);

        /// <summary>
        /// Log a warning exception.
        /// </summary>
        /// <param name="ex">Exception to log</param>
        void Warning(Exception ex)
            => Log(LogLevel.Warning, "{0}", ex.Message);

        /// <summary>
        /// Log an iformational message.
        /// Informations give the user feedback about what is happening.
        /// </summary>
        /// <param name="format">Message, a fomat string that can be handled by the String.Format method</param>
        /// <param name="args">Arguments for formatting</param>
        void Info(string format, params object[] args)
            => Log(LogLevel.Info, format, args);

        /// <summary>
        /// Log a detail. Details are usually not important, so Details are only displayed when
        /// verbose output is enabled.
        /// </summary>
        /// <param name="format">Message, a fomat string that can be handled by the String.Format method</param>
        /// <param name="args">Arguments for formatting</param>
        void Detail(string format, params object[] args)
            => Log(LogLevel.Detail, format, args);
    }
}
