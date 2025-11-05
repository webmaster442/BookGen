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

internal sealed class MicrosoftEditToolDownloader : TooldownloaderBase
{
    public MicrosoftEditToolDownloader(IApiClient apiClient,
                                       RecyclableMemoryStreamManager memoryStreamManager,
                                       ILogger logger)
        : base(apiClient, memoryStreamManager, logger)
    {
    }

    protected override ToolInfo CreateToolInfo()
    {
        return new ToolInfo
        {
            Name = "Microsoft Edit",
            ApproximateSize = "3.6 MiB",
            RepoOwner = "microsoft",
            RepoName = "edit",
            FolderName = "ms-edit",
        };
    }

    protected override ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets)
    {
        return releaseAssets
            .Where(r => r.Name.EndsWith("x86_64-windows.zip"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();
    }
}
