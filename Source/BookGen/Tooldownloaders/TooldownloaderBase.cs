using System.IO.Compression;
using System.Text;

using Bookgen.Lib.Domain.Github;

using BookGen.Infrastructure.Tools;
using BookGen.Vfs;

using Microsoft.IO;

namespace BookGen.Tooldownloaders;

internal abstract class TooldownloaderBase
{
    private readonly IApiClient _apiClient;
    private readonly RecyclableMemoryStreamManager _memoryStreamManager;

    public TooldownloaderBase(IApiClient apiClient,
                              RecyclableMemoryStreamManager memoryStreamManager)
    {
        _apiClient = apiClient;
        _memoryStreamManager = memoryStreamManager;
        ToolInfo = CreateToolInfo();
    }

    protected abstract ToolInfo CreateToolInfo();

    public ToolInfo ToolInfo { get; }

    protected abstract ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets);

    protected virtual Task Extract(IDownloadUi ui, Stream stream)
        => Extractor.ExtractZip(ui, stream, ToolInfo.FolderName);

    public async Task DownloadToolAsync(IDownloadUi ui)
    {
        var downloadUrl = new Uri($"https://api.github.com/repos/{ToolInfo.RepoOwner}/{ToolInfo.RepoName}/releases");
        var releases = await _apiClient.DownloadJsonAsync<Release[]>(downloadUrl);

        var latestRelease = GetReleaseAsset(releases.SelectMany(a => a.Assets));

        if (latestRelease == null)
        {
            ui.Error($"Couldn't find latest release for {ToolInfo.RepoName}");
            return;
        }

        ui.BeginNew($"Downloading {latestRelease.Name}...", latestRelease.Size);

        await using var stream = _memoryStreamManager.GetStream();

        try
        {
            await _apiClient.DownloadFileTo(latestRelease.BrowserDownloadUrl, stream, ui, CancellationToken.None);
            ui.Report(latestRelease.Size);
            stream.Seek(0, SeekOrigin.Begin);        
            await Extract(ui, stream);
        }
        catch (Exception ex)
        {
            ui.Error($"Error downloading {ToolInfo.RepoName}: {ex.Message}");
            return;
        }
    }
}
