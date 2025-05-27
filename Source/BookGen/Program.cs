
using System.Diagnostics;

using BookGen;
using BookGen.Cli;
using BookGen.Cli.Mediator;
using BookGen.Commands;
using BookGen.Infrastructure;
using BookGen.Infrastructure.Loging;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using Spectre.Console;

ProgramInfo info = new();

var argumentList = ProgramConfigurator.ParseGeneralArgs(args, info);

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
CommandRunnerProxy runnerProxy = new();

using SimpleIoC ioc = new();
ioc.RegisterSingleton(logger);
ioc.RegisterSingleton(info);
ioc.RegisterSingleton<ICommandRunnerProxy>(runnerProxy);
ioc.RegisterSingleton<IMediator>(mediator);
ioc.RegisterSingleton<IAssetSource>(ZipAssetSoruce.DefaultAssets());
ioc.RegisterSingleton<IHelpProvider>(new HelpProvider(logger, runnerProxy));
ioc.Register<IWritableFileSystem, FileSystem>();
ioc.Register<IReadOnlyFileSystem, FileSystem>();

ioc.Build();

CommandRunner runner = new(ioc, logger, new CommandRunnerSettings
{
    UnknownCommandCodeAndMessage = (-1, "Unknown command"),
    BadParametersExitCode = 2,
    ExcptionExitCode = -1,
    PlatformNotSupportedExitCode = 4,
    EnableUtf8Output = true,
});

runner.ExceptionHandlerDelegate = OnException;
runner.BeforeRunHook = OnBeforeRun;

runner
    .AddDefaultCommand<HelpCommand>()
    .AddCommandsFrom(typeof(HelpCommand).Assembly);

runnerProxy.ConfigureWith(runner);

HelpProvider helpProvider = new(logger, runnerProxy);
helpProvider.VerifyHelpData();

Stopwatch stopwatch = Stopwatch.StartNew();

int exitCode = await runner.Run(argumentList);

stopwatch.Stop();

if (info.PrintRuntime)
    logger.LogInformation("Execution finished in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

return exitCode;

Task OnBeforeRun(ArgumentsBase @base, IReadOnlyList<string> list)
{
    if (@base is IVerbosablityToggle toggle)
    {
        info.LogLevel = toggle.Verbose ? LogLevel.Debug : LogLevel.Information;
    }
    return Task.CompletedTask;
}


void OnException(Exception exception)
{
    logger.LogCritical(exception.Message);
    CrashDumpFactory.TryCreateCrashDump(exception);
#if DEBUG
    AnsiConsole.WriteException(exception);
#endif
}