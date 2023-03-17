using BookGen;
using BookGen.Cli;
using BookGen.Infrastructure;
using System.IO;

if (UnfinishedUpdateDetected())
{
    Console.WriteLine("Update error. Please reinstall program!");
    return;
}

var argumentList = args.ToList();

ILog log = ProgramConfigurator.ConfigureLog(argumentList);
ProgramInfo info = new();

SimpleIoC ioc = new SimpleIoC();
ioc.RegisterSingleton(log);
ioc.RegisterSingleton(info);


ioc.Build();

// ----------------------------------------------------------------------------

static bool UnfinishedUpdateDetected()
{
    return Directory
        .GetFiles(AppContext.BaseDirectory, "*.*")
        .Any(x => x.EndsWith("_new"));
}