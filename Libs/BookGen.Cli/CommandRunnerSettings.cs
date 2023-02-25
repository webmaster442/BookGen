namespace BookGen.Cli
{
    public class CommandRunnerSettings
    {
        public (int code, string message) UnknownCommandCodeAndMessage { get; init; }

        public CommandRunnerSettings()
        {
            UnknownCommandCodeAndMessage = (-1, "Unknown command");
        }
    }
}
