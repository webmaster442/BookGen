//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace WpLoad.Infrastructure
{
    internal class ConsoleLog : ILog
    {
        private static void Log(string msg, ConsoleColor color)
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = current;
        }

        public void Error(string message) => Log(message, ConsoleColor.Red);

        public void Error(Exception exception)
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.Message);
#if DEBUG
            Console.WriteLine(exception.StackTrace);
#endif
            Console.ForegroundColor = current;
        }
        public void Info(string message) => Log(message, ConsoleColor.White);

        public void Warn(string message) => Log(message, ConsoleColor.Yellow);
    }
}
