//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen;
using BookGen.Commands;
using BookGen.Infrastructure;

using Webmaster442.HttpServerFramework;

if (UnfinishedUpdateDetected())
{
    Console.WriteLine("Update error. Please reinstall program!");
    Environment.Exit(Constants.UpdateError);
}

var argumentList = args.ToList();

(ILog log, IServerLog serverLog) = ProgramConfigurator.ConfigureLog(argumentList);
ProgramInfo info = new();
AppSetting settings = AppSettingHandler.LoadAppSettings() ?? new AppSetting();
var api = new ModuleApi(log, serverLog, settings, info);

SimpleIoC ioc = new();
ioc.RegisterSingleton(log);
ioc.RegisterSingleton(serverLog);
ioc.RegisterSingleton(info);
ioc.RegisterSingleton(settings);
ioc.RegisterSingleton<IAppSetting>(settings);
ioc.RegisterSingleton<IModuleApi>(api);

ioc.Build();

CommandRunner runner = new CommandRunner(ioc, log, new CommandRunnerSettings
{
    UnknownCommandCodeAndMessage = (-1, "Unknown command")
});

api.OnGetAutocompleteItems = runner.GetAutoCompleteItems;
api.OnGetCommandNames = () => runner.CommandNames;
api.OnExecuteModule = (cmd, args) => runner.RunCommand(cmd, args).GetAwaiter().GetResult();

HelpProvider helpProvider = new(log, api);

runner
    .Add<VersionCommand>()
    .Add<ShellCommand>()
    .Add<SubCommandsCommand>()
    .Add<WikiCommand>()
    .Add<SettingsCommand>()
    .Add<GuiCommand>();

helpProvider.VerifyHelpData();

return await runner.Run(argumentList);

// ----------------------------------------------------------------------------

static bool UnfinishedUpdateDetected()
{
    return Directory
        .GetFiles(AppContext.BaseDirectory, "*.*")
        .Any(x => x.EndsWith("_new"));
}