//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Shellprog;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Spectre.Console;

using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

ILogger logger = loggerFactory.CreateLogger("BookGen.Shell");

CommandRunnerProxy runnerProxy = new();

var ioc = new ServiceCollection();
ioc.AddSingleton<IAnsiConsole>(AnsiConsole.Console);
ioc.AddSingleton<ICommandRunnerProxy>(runnerProxy);
ioc.AddSingleton(logger);

using var provider = ioc.BuildServiceProvider();

CommandRunner runner = new(provider, logger, new CommandRunnerSettings
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