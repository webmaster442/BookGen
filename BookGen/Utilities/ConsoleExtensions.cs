//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Utilities
{
    internal static class ConsoleExtensions
    {
        /// <summary>
        /// Log a Timespan in TotalSeconds format to console
        /// </summary>
        /// <param name="ts">Timespan to log</param>
        public static void LogToConsole(this TimeSpan ts)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(ts.TotalSeconds);
            Console.ForegroundColor = color;
        }
    }
}
