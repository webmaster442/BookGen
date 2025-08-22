//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.Github;

using BookGen.Infrastructure.Tools;
using BookGen.Vfs;

using Microsoft.IO;

namespace BookGen.Tooldownloaders;

internal sealed class ChromaDownloader : TooldownloaderBase
{
    public ChromaDownloader(IApiClient apiClient,
                            RecyclableMemoryStreamManager memoryStreamManager)
        : base(apiClient, memoryStreamManager)
    {
    }

    protected override ToolInfo CreateToolInfo()
    {
        return new ToolInfo
        {
            Name = "Chroma",
            ApproximateSize = "8 MiB",
            RepoOwner = "alecthomas",
            RepoName = "chroma",
            FolderName = "chroma",
        };
    }

    protected override Task Extract(IDownloadUi ui, Stream stream)
        => Extractor.ExtractTarGz(ui, stream, ToolInfo.FolderName);

    protected override ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets)
    {
        return releaseAssets
            .Where(r => r.Name.EndsWith("windows-amd64.tar.gz"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();
    }
}
