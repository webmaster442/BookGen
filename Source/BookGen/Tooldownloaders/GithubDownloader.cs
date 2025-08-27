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

internal sealed class GithubDownloader : TooldownloaderBase
{
    public GithubDownloader(IApiClient apiClient, 
                            RecyclableMemoryStreamManager memoryStreamManager,
                            ILogger logger) 
        : base(apiClient, memoryStreamManager, logger)
    {
    }

    protected override ToolInfo CreateToolInfo()
    {
        return new ToolInfo
        {
            Name = "Github CLI",
            ApproximateSize = "38 MiB",
            RepoOwner = "cli",
            RepoName = "cli",
            FolderName = "github-cli",
        };
    }

    protected override ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets)
    {
        return releaseAssets
            .Where(r => r.Name.EndsWith("windows_amd64.zip"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();
    }
}
