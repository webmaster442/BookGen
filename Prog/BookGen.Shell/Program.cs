//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Cli;
using BookGen.Shell.Commands;
using BookGen.Shell.Infrastructure;

using Spectre.Console;

ILog log = new Log();

SimpleIoC ioc = new();
ioc.RegisterSingleton<IAnsiConsole>(AnsiConsole.Console);
ioc.Build();

CommandRunner runner = new CommandRunner(ioc, log, new CommandRunnerSettings
{
    UnknownCommandCodeAndMessage = (-1, "Unknown command"),
    BadParametersExitCode = 2,
    ExcptionExitCode = -1,
    PlatformNotSupportedExitCode = 4,
});

runner.AddCommand<Prompt>();

return await runner.Run(args);