//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Lib;
using BookGen.Lib.Http;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Folder;

[CommandName("folder serve")]
[Description("Starts a local only http server that serves file from the given directory.")]
[ExitCode(ExitCodes.FolderLocked, "A serve command is running in the given folder.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class ServeCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly ILogger _log;
    private readonly IWritableFileSystem _fs;

    public ServeCommand(ILogger log, IWritableFileSystem fs)
    {
        _log = log;
        _fs = fs;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context, CancellationToken token)
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
        await using (var runner = new ConsoleHttpServerRunner(HttpServerFactory.CreateServerForDirectoryHosting(arguments.Directory, _log)))
        {
            var serverurls = string.Join(' ', runner.Server.GetListenUrls());
            var qrcodes = string.Join(' ', runner.Server.GetListenUrls().Select(x => $"{x}/qrcodelink"));

            _log.LogInformation("Serving: {directory}", arguments.Directory);
            _log.LogInformation("Server running on {urls}", serverurls);
            _log.LogInformation("To get QR code for another device visit: {qrcodes}", qrcodes);

            await runner.RunServer();
        }
#pragma warning restore CA2000 // Dispose objects before losing scope

        return ExitCodes.Success;
    }
}
