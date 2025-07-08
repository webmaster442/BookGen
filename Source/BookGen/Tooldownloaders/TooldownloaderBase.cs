using System.IO.Compression;
using System.Text;

using Bookgen.Lib.Domain.Github;

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
    }

    public abstract string ToolName { get; }

    public abstract string ApproximateSize { get; }

    protected abstract (string owner, string repo) GetRepository();

    protected abstract ReleaseAsset? GetReleaseAsset(IEnumerable<ReleaseAsset> releaseAssets);

    protected virtual async Task Extract(IDownloadUi ui, Stream stream)
    {
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read, true, Encoding.UTF8);

        ui.BeginNew("Extracting...", archive.Entries.Sum(e => e.Length));

        foreach (var entry in archive.Entries)
        {
            if (string.IsNullOrEmpty(entry.Name))
            {
                // Skip directories
                continue;
            }

            string outputPath = GetEntryOutputPath(entry);
            string? directory = Path.GetDirectoryName(outputPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using var targetStream = File.Create(outputPath);
            await using var source = entry.Open();
            await source.CopyToAsync(targetStream, CancellationToken.None);

            ui.Report(entry.Length);
        }
    }

    protected virtual string GetEntryOutputPath(ZipArchiveEntry zipArchiveEntry)
        => Path.Combine(AppContext.BaseDirectory, "tools", zipArchiveEntry.FullName);

    public async Task DownloadToolAsync(IDownloadUi ui)
    {
        var (owner, repo) = GetRepository();
        var downloadUrl = new Uri($"https://api.github.com/repos/{owner}/{repo}/releases");
        var releases = await _apiClient.DownloadJsonAsync<Release[]>(downloadUrl);

        var latestRelease = GetReleaseAsset(releases.SelectMany(a => a.Assets));

        if (latestRelease == null)
        {
            ui.Error($"Couldn't find latest release for {repo}");
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
            ui.Error($"Error downloading {repo}: {ex.Message}");
            return;
        }
    }
}
