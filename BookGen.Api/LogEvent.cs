//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api
{
    /// <summary>
    /// Log event args
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Log level
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Log message
        /// </summary>
        public string Message { get; }


        /// <summary>
        /// Creates a new instance of LogEventArgs
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="message">Message</param>
        public LogEventArgs(LogLevel logLevel, string message)
        {
            LogLevel = logLevel;
            Message = message;
        }
    }
}
