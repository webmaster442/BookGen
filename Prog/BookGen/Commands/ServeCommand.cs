using BookGen.CommandArguments;
using BookGen.Framework;
using BookGen.Framework.Server;
using BookGen.Infrastructure;

using Webmaster442.HttpServerFramework;

namespace BookGen.Commands;

[CommandName("serve")]
internal class ServeCommand : Command<BookGenArgumentBase>
{
    private readonly ILog _log;
    private readonly IServerLog _serverLog;

    public ServeCommand(ILog log, IServerLog serverLog)
    {
        _log = log;
        _serverLog = serverLog;
    }

    public override int Execute(BookGenArgumentBase arguments, string[] context)
    {
        _log.LogLevel = arguments.Verbose ? Api.LogLevel.Detail : Api.LogLevel.Info;

        _log.CheckLockFileExistsAndExitWhenNeeded(arguments.Directory);

        using (var l = new FolderLock(arguments.Directory))
        {
            using (HttpServer? server = HttpServerFactory.CreateServerForServModule(_serverLog, arguments.Directory))
            {
                server.Start();
                Console.WriteLine("Serving: {0}", arguments.Directory);
                Console.WriteLine("Server running on http://localhost:8081");
                Console.WriteLine("Press a key to exit...");
                Console.ReadLine();
                server.Stop();
            }
        }

        return Constants.Succes;
    }
}
