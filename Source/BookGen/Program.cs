//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen;
using BookGen.Cli;
using BookGen.Commands;
using BookGen.Infrastructure;
using BookGen.Infrastructure.Loging;
using BookGen.Vfs;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Spectre.Console;

ProgramInfo info = new();

var argumentList = ProgramConfigurator.ParseGeneralArgs(args, info);

using ILoggerFactory factory = LoggerFactory
    .Create(builder =>
    {
        builder.ClearProviders();
        builder.AddFilter(level => level >= info.LogLevel);
        if (info.JsonLogging)
        {
            builder.AddJsonConsole();
        }
        else
        {
            builder.AddProvider(new ConsoleLogProvider());
        }
        if (info.LogToFile)
        {
            builder.AddProvider(new FileLoggerProvider());
        }
    });

ILogger logger = factory.CreateLogger("Bookgen");
CommandRunnerProxy runnerProxy = new();

var ioc = new ServiceCollection();
ioc.AddSingleton(logger);
ioc.AddSingleton(info);
ioc.AddSingleton<ICommandRunnerProxy>(runnerProxy);
ioc.AddSingleton<IAssetSource>(ZipAssetSoruce.DefaultAssets());
ioc.AddSingleton<IHelpProvider>(new HelpProvider(logger, runnerProxy));
ioc.AddTransient<IWritableFileSystem, FileSystem>();
ioc.AddTransient<IReadOnlyFileSystem, FileSystem>();
ioc.AddTransient<IApiClient, ApiClient>();

using var provider = ioc.BuildServiceProvider();

CommandRunner runner = new(provider, logger, new CommandRunnerSettings
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
#if DEBUG
        info.LogLevel = toggle.Verbose || Debugger.IsAttached ? LogLevel.Debug : LogLevel.Information;
#else
        info.LogLevel = toggle.Verbose ? LogLevel.Debug : LogLevel.Information;
#endif
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