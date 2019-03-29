//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Core.Contracts
{
    public interface ILog : IDisposable
    {
        void ConfigureFile(string path);
        void Log(LogLevel logLevel, string format, params object[] args);
        void Critical(string format, params object[] args);
        void Critical(Exception ex);
        void Warning(string format, params object[] args);
        void Warning(Exception ex);
        void Info(string format, params object[] args);
        void Detail(string format, params object[] args);
    }
}
