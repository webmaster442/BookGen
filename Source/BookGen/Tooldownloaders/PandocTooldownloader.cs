using Bookgen.Lib.Domain.Github;

using BookGen.Vfs;

using Microsoft.IO;

namespace BookGen.Tooldownloaders;

internal class PandocTooldownloader : TooldownloaderBase
{
    public PandocTooldownloader(IApiClient apiClient,
                                RecyclableMemoryStreamManager memoryStreamManager)
        : base(apiClient, memoryStreamManager)
    {
    }

    public override string ToolName => "Pandoc";

    protected override (string owner, string repo) GetRepository() => ("jgm", "pandoc");

    protected override ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets)
    {
        return releaseAssets
            .Where(r => r.Name.EndsWith("windows-x86_64.zip"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();
    }
}
