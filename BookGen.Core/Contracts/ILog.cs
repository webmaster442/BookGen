//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Core.Contracts
{
    /// <summary>
    /// Interface for logging
    /// </summary>
    public interface ILog
    {
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
            => Log(LogLevel.Info, format, args);
    }
}
