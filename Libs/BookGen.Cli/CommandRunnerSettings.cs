//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
