using System.Text.Json;

namespace BookGen.Vfs;

public interface IApiClient : IDisposable
{
    Task<T> DownloadJsonAsync<T>(Uri url, JsonSerializerOptions? options = null) where T : class;
    Task DownloadFileTo(Uri url, Stream target, IProgress<long> progress, CancellationToken cancellationToken);
}
