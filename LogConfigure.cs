//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;

namespace BookGen
{
    internal static class LogConfigure
    {
        public static void ConfigureNlog()
        {
            var file = Path.Combine(Environment.CurrentDirectory, "boogken.log");
            Console.WriteLine("Log enabled. Output file: {0}", file);
            var configuration = new LoggingConfiguration();
            var logfile = new FileTarget("logfile") { FileName = file };

            configuration.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);

            LogManager.Configuration = configuration;

            Logger log = LogManager.GetCurrentClassLogger();
        }

    }
}
