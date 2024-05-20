//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;

namespace BookGen.Shell.Infrastructure;

internal class Log : ILog
{
    public LogLevel LogLevel { get; set; }

    public event EventHandler<LogEventArgs>? OnLogWritten;

    void ILog.Log(LogLevel logLevel, string format, params object[] args)
    {
        string msg;
        if (args.Length == 0)
            msg = format;
        else
            msg = string.Format(format, args);
        OnLogWritten?.Invoke(this, new LogEventArgs(logLevel, msg));
    }
}
