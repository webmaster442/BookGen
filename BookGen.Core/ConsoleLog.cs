//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
using System.Diagnostics;

namespace BookGen.Core
{
    public sealed class ConsoleLog : ILog
    {
        private readonly LogLevel _logLevel;

        public ConsoleLog(LogLevel level = LogLevel.Info)
        {
            _logLevel = level;
        }

        private ConsoleColor GetConsoleColor(LogLevel logLevel)
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

            if (logLevel <= _logLevel)
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
    }
}
