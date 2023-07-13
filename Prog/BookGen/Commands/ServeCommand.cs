//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
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

        using (HttpServer? server = HttpServerFactory.CreateServerForServModule(_serverLog, arguments.Directory))
        {
            server.Start();
            _log.Info("Serving: {0}", arguments.Directory);
            _log.Info("Server running on http://localhost:8081");
            _log.Info("To get QR code for another device visit: http://localhost:8081/qrcodelink");
            _log.Info("Press a key to exit...");
            Console.ReadLine();
            server.Stop();
        }

        return Constants.Succes;
    }
}
