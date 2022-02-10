namespace WpLoad.Infrastructure
{
    /// <summary>
    /// Logger interface
    /// </summary>
    internal interface ILog
    {
        void Info(string message);
        void Info(string message, string[] arguments);
        void Warn(string message);
        void Warn(string message, string[] arguments);
        void Error(string message);
        void Error(Exception exception);
    }
}
