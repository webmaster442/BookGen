﻿using System;

namespace BookGen.Utilities
{
    internal static class ConsoleExtensions
    {
        /// <summary>
        /// Log an error to console
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static void LogToConsole(this Exception ex)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Log a Timespan in TotalSeconds format to console
        /// </summary>
        /// <param name="ts">Timespan to log</param>
        public static void LogToConsole(this TimeSpan ts)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ts.TotalSeconds);
            Console.ForegroundColor = color;
        }
    }
}
