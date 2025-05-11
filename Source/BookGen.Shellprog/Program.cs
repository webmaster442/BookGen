using BookGen.Cli;
using BookGen.Shell.Commands;
using BookGen.Shellprog;

using Microsoft.Extensions.Logging;

using Spectre.Console;

ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("BookGen.Shell");

CommandNameProvider commandNameProvider = new();

using SimpleIoC ioc = new();
ioc.RegisterSingleton<IAnsiConsole>(AnsiConsole.Console);
ioc.RegisterSingleton(commandNameProvider);
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
    .AddCommand<CdgCommand>()
    .AddCommand<GitAutoCompleteCommand>()
    .AddCommand<OrganizeCommand>();

commandNameProvider.CommandNames = runner.CommandNames;

return await runner.Run(args);