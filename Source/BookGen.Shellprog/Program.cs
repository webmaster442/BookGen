using BookGen.Cli;
using BookGen.Shellprog;

using Microsoft.Extensions.Logging;

using Spectre.Console;

using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

ILogger logger = loggerFactory.CreateLogger("BookGen.Shell");

CommandRunnerProxy runnerProxy = new();

using SimpleIoC ioc = new();
ioc.RegisterSingleton<IAnsiConsole>(AnsiConsole.Console);
ioc.RegisterSingleton<ICommandRunnerProxy>(runnerProxy);
ioc.RegisterSingleton(logger);
ioc.Build();

CommandRunner runner = new(ioc, logger, new CommandRunnerSettings
{
    UnknownCommandCodeAndMessage = (-1, "Unknown command"),
    BadParametersExitCode = 2,
    ExcptionExitCode = -1,
    PlatformNotSupportedExitCode = 4,
    EnableUtf8Output = true,
});

runner
    .AddDefaultCommand<CommandListCommand>()
    .AddCommand<PromptCommand>()
    .AddCommand<RepoWeb>()
    .AddCommand<CdgCommand>()
    .AddCommand<GitAutoCompleteCommand>()
    .AddCommand<OrganizeCommand>();

runnerProxy.ConfigureWith(runner);

return await runner.Run(args);