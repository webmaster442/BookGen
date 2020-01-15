//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Api
{
    /// <summary>
    /// Interface for logging
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="format">A fomart string that can be handled by String.Format</param>
        /// <param name="args">Arguments for formatting</param>
        void Log(LogLevel logLevel, string format, params object[] args);
        void Critical(string format, params object[] args)
            => Log(LogLevel.Critical, format, args);
        void Critical(Exception ex)
            => Log(LogLevel.Critical, "{0}\r\n{1}", ex.Message, ex.StackTrace);
        void Warning(string format, params object[] args)
            => Log(LogLevel.Warning, format, args);
        void Warning(Exception ex)
            => Log(LogLevel.Warning, "{0}", ex.Message);
        void Info(string format, params object[] args)
            => Log(LogLevel.Info, format, args);
        void Detail(string format, params object[] args)
            => Log(LogLevel.Detail, format, args);
    }
}
