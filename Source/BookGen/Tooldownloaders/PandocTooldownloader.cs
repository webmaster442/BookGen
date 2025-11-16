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

internal sealed class PandocTooldownloader : TooldownloaderBase
{
    public PandocTooldownloader(IApiClient apiClient,
                                RecyclableMemoryStreamManager memoryStreamManager,
                                ILogger logger)
        : base(apiClient, memoryStreamManager, logger)
    {
    }

    protected override ToolInfo CreateToolInfo()
    {
        return new ToolInfo
        {
            Name = "Pandoc",
            ApproximateSize = "217 MiB",
            RepoOwner = "jgm",
            RepoName = "pandoc",
            FolderName = "pandoc",
        };
    }

    protected override ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets)
    {
        return releaseAssets
            .Where(r => r.Name.EndsWith("windows-x86_64.zip"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();
    }
}
