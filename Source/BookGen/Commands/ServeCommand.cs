//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib;
using Bookgen.Lib.Http;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("serve")]
internal class ServeCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly ILogger _log;
    private readonly IWritableFileSystem _fs;

    public ServeCommand(ILogger log, IWritableFileSystem fs)
    {
        _log = log;
        _fs = fs;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        _fs.Scope = arguments.Directory;

        using var folderLock = new FolderLock(_fs, FileNameConstants.LockFile);

        if (!folderLock.Initialize())
        {
            _log.LogError("Failed to initialize folder lock. Another instance may be running or the directory is not writable.");
            return ExitCodes.FolderLocked;
        }

#pragma warning disable CA2000 // Dispose objects before losing scope
        //runner is responsible for disposing the server
        await using (var runner = new ConsoleHttpServerRunner(ServerFactory.CreateServerForDirectoryHosting(arguments.Directory)))
        {
            var serverurls = string.Join(' ', runner.Server.GetListenUrls());
            var qrcodes = string.Join(' ', runner.Server.GetListenUrls().Select(x => $"{x}/qrcodelink"));

            _log.LogInformation("Serving: {directory}", arguments.Directory);
            _log.LogInformation("Server running on {urls}", serverurls);
            _log.LogInformation("To get QR code for another device visit: {qrcodes}", qrcodes);

            await runner.RunServer();
        }
#pragma warning restore CA2000 // Dispose objects before losing scope

        return ExitCodes.Succes;
    }
}
