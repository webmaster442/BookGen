using System.Text.Json;

namespace BookGen.Vfs;

public interface IApiClient : IDisposable
{
    Task<T> DownloadJsonAsync<T>(Uri url, JsonSerializerOptions? options) where T : class;
    Task DownloadFileTo(Uri url, string targetPath, IProgress<long> progress, CancellationToken cancellationToken);
}
