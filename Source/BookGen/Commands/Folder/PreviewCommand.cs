//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Lib;
using BookGen.Lib.AppSettings;
using BookGen.Lib.Http;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Folder;

[CommandName("folder preview")]
internal sealed class PreviewCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _fs;
    private readonly ILogger _log;
    private readonly IProgramPathResolver _programPathResolver;
    private readonly IAssetSource _assetSource;

    public PreviewCommand(IWritableFileSystem source,
                          ILogger logger,
                          IProgramPathResolver programPathResolver,
                          IAssetSource assetSource)
    {
        _fs = source;
        _log = logger;
        _programPathResolver = programPathResolver;
        _assetSource = assetSource;
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

        await using (var runner = new ConsoleHttpServerRunner(HttpServerFactory.CreateServerForPreview(_fs, _log, _programPathResolver, _assetSource)))
        {
            var serverurls = string.Join(' ', runner.Server.GetListenUrls());
            _log.LogInformation("Preview server working in: {directory}", arguments.Directory);
            _log.LogInformation("Preview server running on {urls}", serverurls);
            await runner.RunServer();
        }

        return ExitCodes.Success;
    }
}
