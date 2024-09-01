//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Framework;
using BookGen.Framework.Server;
using BookGen.Infrastructure;

using Webmaster442.HttpServerFramework;

namespace BookGen.Commands;

[CommandName("serve")]
internal class ServeCommand : Command<BookGenArgumentBase>
{
    private readonly ILogger _log;
    private readonly IMutexFolderLock _folderLock;

    public ServeCommand(ILogger log, IMutexFolderLock folderLock)
    {
        _log = log;
        _folderLock = folderLock;
    }

    public override int Execute(BookGenArgumentBase arguments, string[] context)
    {
        _log.EnableVerboseLogingIfRequested(arguments);

        _folderLock.CheckLockFileExistsAndExitWhenNeeded(_log, arguments.Directory);

        using (HttpServer? server = HttpServerFactory.CreateServerForServModule(_log, arguments.Directory))
        {
            server.Start();
            _log.LogInformation("Serving: {directory}", arguments.Directory);
            _log.LogInformation("Server running on http://localhost:8081");
            _log.LogInformation("To get QR code for another device visit: http://localhost:8081/qrcodelink");
            _log.LogInformation("Press a key to exit...");
            Console.ReadLine();
            server.Stop();
        }

        return Constants.Succes;
    }
}
