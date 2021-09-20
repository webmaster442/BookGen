//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
using System.Collections.Generic;
using Webmaster442.HttpServerFramework;

namespace BookGen.Framework
{
    internal class JsonLog : ILog, IServerLog
    {
        private readonly List<LogEntry> _entries;

        public JsonLog()
        {
            _entries = new List<LogEntry>(20);
        }

        public LogLevel LogLevel { get; set; }

        public void Log(LogLevel logLevel, string format, params object[] args)
        {
            _entries.Add(new LogEntry
            {
                Message = string.Format(format, args),
                LogLevel = logLevel,
                TimeStamp = DateTime.UtcNow,
            });
        }

        public void PrintLine(string str)
        {
            Log(LogLevel.PrintLine, str, Array.Empty<string>());
        }

        public void PrintLine(object obj)
        {
            Log(LogLevel.PrintLine, obj.ToString() ?? string.Empty, Array.Empty<string>());
        }
    }
}
