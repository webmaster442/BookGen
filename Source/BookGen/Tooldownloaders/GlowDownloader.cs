//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Infrastructure.Tools;
using BookGen.Lib.Domain.Github;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace BookGen.Tooldownloaders;

internal sealed class GlowDownloader : TooldownloaderBase
{
    public GlowDownloader(IApiClient apiClient,
                          RecyclableMemoryStreamManager memoryStreamManager,
                          ILogger log)
        : base(apiClient, memoryStreamManager, log)
    {
        ToolInfo = new ToolInfo
        {
            Name = "Glow",
            ApproximateSize = "18 MiB",
            RepoOwner = "charmbracelet",
            RepoName = "glow",
            FolderName = "glow",
        };
    }

    public override ToolInfo ToolInfo { get; }

    protected override ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets)
    {
        return releaseAssets
            .Where(r => r.Name.EndsWith("Windows_x86_64.zip"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();
    }
}
