namespace WpLoad.Infrastructure
{
    internal class ConsoleLog : ILog
    {
        private static void Log(string msg, ConsoleColor color)
        {
            var current = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = current;
        }

        public void Error(string message) => Log(message, ConsoleColor.Red);

        public void Error(Exception exception)
        {
            var current = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.Message);
#if DEBUG
            Console.WriteLine(exception.StackTrace);
#endif
            Console.ForegroundColor = current;
        }
        public void Info(string message) => Log(message, ConsoleColor.White);

        public void Info(string message, string[] arguments) => Log(string.Format(message, arguments), ConsoleColor.White);

        public void Warn(string message) => Log(message, ConsoleColor.Yellow);

        public void Warn(string message, string[] arguments) => Log(string.Format(message, arguments), ConsoleColor.Yellow);
    }
}
