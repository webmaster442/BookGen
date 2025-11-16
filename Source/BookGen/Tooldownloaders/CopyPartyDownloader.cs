//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.Github;

using BookGen.Infrastructure.Tools;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace BookGen.Tooldownloaders;

internal sealed class CopyPartyDownloader : TooldownloaderBase
{
    public CopyPartyDownloader(IApiClient apiClient,
                              RecyclableMemoryStreamManager memoryStreamManager,
                              ILogger logger)
        : base(apiClient, memoryStreamManager, logger)
    {
    }

    protected override ToolInfo CreateToolInfo()
    {
        return new ToolInfo
        {
            Name = "Copyparty.exe",
            ApproximateSize = "13 MiB",
            RepoOwner = "9001",
            RepoName = "copyparty",
            FolderName = "copyparty",
        };
    }

    protected override ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets)
    {
        return releaseAssets
            .Where(r => r.Name == "copyparty.exe")
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();
    }

    protected override Task Extract(IDownloadUi ui, Stream stream)
        => Extractor.Copy(ui, stream, ToolInfo.FolderName, "copyparty.exe");
}
