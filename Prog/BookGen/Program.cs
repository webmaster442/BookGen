//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen;
using BookGen.Commands;
using BookGen.Gui;
using BookGen.Infrastructure;

using Webmaster442.HttpServerFramework;

if (IsUnfinishedUpdateDetected())
{
    Console.WriteLine("Update error. Please reinstall program!");
    Environment.Exit(Constants.UpdateError);
}

var argumentList = args.ToList();

ProgramConfigurator.WaitForDebugger(argumentList);

(ILog log, IServerLog serverLog) = ProgramConfigurator.ConfigureLog(argumentList);
ProgramInfo info = new();

var timeProvider = new TimeProviderImplementation();

AppSetting settings = AppSettingHandler.LoadAppSettings() ?? new AppSetting();
var api = new ModuleApi(log, serverLog, settings, info, timeProvider);

SimpleIoC ioc = new();
ioc.RegisterSingleton<ITerminal, Terminal>();
ioc.RegisterSingleton(log);
ioc.RegisterSingleton(serverLog);
ioc.RegisterSingleton(info);
ioc.RegisterSingleton(settings);
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

return await runner.Run(argumentList);

// ----------------------------------------------------------------------------

static bool IsUnfinishedUpdateDetected()
{
    return Directory
        .GetFiles(AppContext.BaseDirectory, "*.*")
        .Any(x => x.EndsWith("_new"));
}