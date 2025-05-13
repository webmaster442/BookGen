using BookGen;
using BookGen.Cli;
using BookGen.Cli.Mediator;
using BookGen.Commands;
using BookGen.Infrastructure;
using BookGen.Infrastructure.Loging;

using Microsoft.Extensions.Logging;

var argumentList = args.ToList();

ProgramInfo info = new();

ProgramConfigurator.AttachDebugger(argumentList);
ProgramConfigurator.WaitForDebugger(argumentList);
ProgramConfigurator.ConfigureLog(info, argumentList);

using Mediator mediator = new();

using ILoggerFactory factory = LoggerFactory
    .Create(builder =>
    {
        builder.ClearProviders();
        if (info.JsonLogging)
        {
            builder.AddJsonConsole();
        }
        else
        {
            builder.AddProvider(new ConsoleLogProvider(mediator));
            builder.AddProvider(new DebugLoggerProvider()).AddFilter((Category, level) => level == LogLevel.Debug);
        }
        if (info.LogToFile)
        {
            builder.AddProvider(new FileLoggerProvider());
        }
        builder.AddFilter((Category, level) => level >= info.LogLevel);
    });

ILogger logger = factory.CreateLogger("Bookgen");
CommandNameProvider commandNameProvider = new();

using SimpleIoC ioc = new();
ioc.RegisterSingleton(logger);
ioc.RegisterSingleton(info);
ioc.RegisterSingleton(commandNameProvider);
ioc.RegisterSingleton<IMediator>(mediator);

ioc.Build();

CommandRunner runner = new CommandRunner(ioc, logger, new CommandRunnerSettings
{
    UnknownCommandCodeAndMessage = (-1, "Unknown command"),
    BadParametersExitCode = 2,
    ExcptionExitCode = -1,
    PlatformNotSupportedExitCode = 4,
});

runner
    .AddDefaultCommand<HelpCommand>()
    .AddCommandsFrom(typeof(HelpCommand).Assembly);

commandNameProvider.CommandNames = runner.CommandNames;

HelpProvider helpProvider = new(logger, commandNameProvider);
helpProvider.VerifyHelpData();

return await runner.Run(argumentList);