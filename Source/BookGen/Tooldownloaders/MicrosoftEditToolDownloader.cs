using System.IO.Compression;

using Bookgen.Lib.Domain.Github;

using BookGen.Vfs;

using Microsoft.IO;

namespace BookGen.Tooldownloaders;

internal sealed class MicrosoftEditToolDownloader : TooldownloaderBase
{
    public MicrosoftEditToolDownloader(IApiClient apiClient,
                                       RecyclableMemoryStreamManager memoryStreamManager)
        : base(apiClient, memoryStreamManager)
    {
    }
    public override string ToolName => "Microsoft Edit";

    public override string ApproximateSize => "3.6 MiB";

    protected override (string owner, string repo) GetRepository() => ("microsoft", "edit");

    protected override ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets)
    {
        return releaseAssets
            .Where(r => r.Name.EndsWith("x86_64-windows.zip"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();
    }
    protected override string GetEntryOutputPath(ZipArchiveEntry zipArchiveEntry)
        => Path.Combine(AppContext.BaseDirectory, "tools", "edit", zipArchiveEntry.Name);
}