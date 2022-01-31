//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
using System.Diagnostics;
using System.Reflection;
using Webmaster442.HttpServerFramework;

namespace BookGen.Framework
{
    public sealed class ConsoleLog : ILog, IServerLog
    {
        public ConsoleLog(LogLevel level = LogLevel.Info)
        {
            LogLevel = level;
        }

        void IServerLog.Critical(Exception ex)
        {
            Log(LogLevel.Critical, ex.Message);
        }

        void IServerLog.Info(string format, params object[] args)
        {
            Log(LogLevel.Info, format, args);
        }

        void IServerLog.Warning(string format, params object[] args)
        {
            Log(LogLevel.Warning, format, args);
        }

        public LogLevel LogLevel { get; set; }

        private static ConsoleColor GetConsoleColor(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Info => ConsoleColor.Gray,
                LogLevel.Critical => ConsoleColor.Red,
                _ => ConsoleColor.White,
            };
        }

        public void Log(LogLevel logLevel, string format, params object[] args)
        {
            string text = string.Format(format, args);
            string line = string.Format("{0} | {1} | {2}", DateTime.Now.ToShortTimeString(), logLevel, text);

            if (logLevel <= LogLevel)
            {
                Console.ForegroundColor = GetConsoleColor(logLevel);
                Console.WriteLine(line);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
#if DEBUG
            else
            {
                Debug.WriteLine(line);
            }
#endif
        }

        public void PrintLine(string str)
        {
            Console.WriteLine(str);
        }

        public void PrintLine(object obj)
        {
            Console.WriteLine(obj);
        }
    }
}
