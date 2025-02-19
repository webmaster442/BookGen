﻿//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen;
using BookGen.Cli.Mediator;
using BookGen.Commands;
using BookGen.Framework;
using BookGen.Gui;
using BookGen.Infrastructure;

var argumentList = args.ToList();

ProgramInfo info = new();

ProgramConfigurator.AttachDebugger(argumentList);
ProgramConfigurator.WaitForDebugger(argumentList);
ProgramConfigurator.ConfigureLog(info, argumentList);

using Mediator mediator = new();

ILogger log = LoggerFactory
    .Create(builder =>
    {
        builder.ClearProviders();
        if (info.JsonLogging)
        {
            builder.AddJsonConsole();
        }
        else
        {
            builder.AddProvider(new BookGen.Gui.ConsoleLogProvider(mediator));
            builder.AddProvider(new DebugLoggerProvider()).AddFilter((Category, level) => level == LogLevel.Debug);
        }
        if (info.LogToFile)
        {
            builder.AddProvider(new FileLoggerProvider());
        }
        builder.AddFilter((Category, level) => level >= info.LogLevel);
    })
    .CreateLogger("BookGen");


var timeProvider = new TimeProviderImplementation();

AppSetting settings = await AppSettingHandler.LoadAppSettingsAsync();

var api = new ModuleApi(log, mediator, settings, info, timeProvider);

using SimpleIoC ioc = new();
ioc.RegisterSingleton<ITerminal, Terminal>();
ioc.RegisterSingleton<IMutexFolderLock, MutexFolderLock>();
ioc.RegisterSingleton(log);
ioc.RegisterSingleton(info);
ioc.RegisterSingleton(settings);
ioc.RegisterSingleton<IMediator>(mediator);
ioc.RegisterSingleton<IAppSetting>(settings);
ioc.RegisterSingleton<IModuleApi>(api);
ioc.RegisterSingleton<TimeProvider>(timeProvider);
ioc.Register<BookGenTaskRunner>();

ioc.Build();

CommandRunner runner = new CommandRunner(ioc, log, new CommandRunnerSettings
{
    UnknownCommandCodeAndMessage = (-1, "Unknown command"),
    BadParametersExitCode = 2,
    ExcptionExitCode = -1,
    PlatformNotSupportedExitCode = 4,
});

api.OnGetAutocompleteItems = runner.GetAutoCompleteItems;
api.OnGetCommandNames = () => runner.CommandNames;
api.OnExecuteModule = (cmd, args) => runner.RunCommand(cmd, args).GetAwaiter().GetResult();

HelpProvider helpProvider = new(log, api);

runner
    .AddDefaultCommand<HelpCommand>()
    .AddCommandsFrom(typeof(HelpCommand).Assembly);

helpProvider.VerifyHelpData();
helpProvider.RegisterCallback("build", HelpCallbacks.DocumentBuildActions);
ioc.RegisterSingleton<IHelpProvider>(helpProvider);

int exitCode = await runner.Run(argumentList);

return exitCode;