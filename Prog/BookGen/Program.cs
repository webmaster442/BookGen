using BookGen;
using BookGen.Cli;
using BookGen.Commands;
using BookGen.Infrastructure;
using BookGen.Interfaces;
using System.IO;

if (UnfinishedUpdateDetected())
{
    Console.WriteLine("Update error. Please reinstall program!");
    Environment.Exit(Constants.UpdateError);
}

var argumentList = args.ToList();

ILog log = ProgramConfigurator.ConfigureLog(argumentList);
ProgramInfo info = new();
AppSetting settings = AppSettingHandler.LoadAppSettings() ?? new AppSetting();

SimpleIoC ioc = new SimpleIoC();
ioc.RegisterSingleton(log);
ioc.RegisterSingleton(info);
ioc.RegisterSingleton(settings);
ioc.RegisterSingleton<IAppSetting>(settings);

ioc.Build();

CommandRunner runner = new CommandRunner(ioc, log, new CommandRunnerSettings
{
    UnknownCommandCodeAndMessage = (-1, "Unknown command")
});

runner
    .Add<VersionCommand>()
    .Add<ShellCommand>()
    .Add<SubCommandsCommand>()
    .Add<WikiCommand>()
    .Add<SettingsCommand>();

return await runner.Run(argumentList);

// ----------------------------------------------------------------------------

static bool UnfinishedUpdateDetected()
{
    return Directory
        .GetFiles(AppContext.BaseDirectory, "*.*")
        .Any(x => x.EndsWith("_new"));
}