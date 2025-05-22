//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Http;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("serve")]
internal class ServeCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly ILogger _log;
    private readonly IMutexFolderLock _folderLock;
    private readonly ProgramInfo _programInfo;

    public ServeCommand(ILogger log, IMutexFolderLock folderLock, ProgramInfo programInfo)
    {
        _log = log;
        _folderLock = folderLock;
        _programInfo = programInfo;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, string[] context)
    {
        _programInfo.EnableVerboseLogingIfRequested(arguments);

        _folderLock.CheckLockFileExistsAndExitWhenNeeded(_log, arguments.Directory);

        var server = ServerFactory.CreateServerForDirectoryHosting(arguments.Directory);

        using (var runner = new ConsoleHttpServerRunner(server))
        {
            var serverurls = string.Join(' ', server.GetListenUrls());
            var qrcodes = string.Join(' ', server.GetListenUrls().Select(x => $"{x}/qrcodelink"));

            _log.LogInformation("Serving: {directory}", arguments.Directory);
            _log.LogInformation("Server running on {urls}", serverurls);
            _log.LogInformation("To get QR code for another device visit: {qrcodes}", qrcodes);

            await runner.RunServer();
        }

        return ExitCodes.Succes;
    }
}
