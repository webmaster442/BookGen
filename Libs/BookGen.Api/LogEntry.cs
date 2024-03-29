﻿//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api
{
    /// <summary>
    /// Represents a log enty
    /// </summary>
    public sealed class LogEntry
    {
        /// <summary>
        /// Message log level
        /// </summary>
        public LogLevel LogLevel { get; init; }
        /// <summary>
        /// Log message content
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// Timestamp in UTC
        /// </summary>
        public DateTime TimeStamp { get; init; }

        /// <summary>
        /// Creates a new instance of LogEntry
        /// </summary>
        public LogEntry()
        {
            Message = string.Empty;
        }
    }
}
