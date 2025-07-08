using System.Net;
using System.Text.Json;

namespace BookGen.Vfs;

public sealed class ApiClient : IApiClient
{
    private readonly HttpClient _client;
    private bool _disposed;

    public ApiClient()
    {
        _client = new HttpClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _disposed = true;
    }

    private Exception CreateExceptionBasedOnStatusCode(HttpResponseMessage response, Uri url)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.NotFound => new InvalidOperationException($"Resource not found at {url}"),
            HttpStatusCode.Unauthorized => new UnauthorizedAccessException($"Unauthorized access to {url}"),
            HttpStatusCode.Forbidden => new InvalidOperationException($"Forbidden access to {url}"),
            _ => new InvalidOperationException($"Failed to download JSON from {url}: {response.ReasonPhrase}"),
        };
    }

    public async Task<T> DownloadJsonAsync<T>(Uri url, JsonSerializerOptions? options = null) where T : class
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(ApiClient));

        options ??= JsonOptions.SerializerOptions;

        using HttpResponseMessage? response = await _client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            await using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(stream, options)
                ?? throw new InvalidOperationException($"Coudln't deserialize response as {typeof(T)}");
        }

        throw CreateExceptionBasedOnStatusCode(response, url);
    }

    public async Task DownloadFileTo(Uri url, Stream target, IProgress<long> progress, CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(ApiClient));

        using HttpResponseMessage? response = await _client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            await using var source = await response.Content.ReadAsStreamAsync();

            byte[] buffer = new byte[8192];
            int read = 0;
            long totalRead = 0;

            do
            {
                read = await source.ReadAsync(buffer, cancellationToken);
                await target.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
                totalRead += read;
                progress.Report(totalRead);
            }
            while (read > 0 && !cancellationToken.IsCancellationRequested);

            return;
        }

        throw CreateExceptionBasedOnStatusCode(response, url);
    }
}
