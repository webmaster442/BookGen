//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net;

namespace BookGen.Infrastructure.Web;

public sealed class BookGenHttpClient : IDisposable
{
    private readonly HttpClient _client;
    private bool _disposed;

    public BookGenHttpClient()
    {
        _client = new HttpClient();
    }

    public async Task<(HttpStatusCode code, string resultString)> TryDownload(Uri url)
    {
        using HttpResponseMessage? response = await _client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return (response.StatusCode, content);
        }
        else
        {
            return (response.StatusCode, string.Empty);
        }
    }

    public async Task<HttpStatusCode> DownloadTo(Uri url, Stream target)
    {
        using HttpResponseMessage? response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            await stream.CopyToAsync(target);
        }
        return response.StatusCode;
    }

    public static bool IsSuccessfullRequest(HttpStatusCode code)
    {
        int c = (int)code;
        return c >= 200 && c <= 300;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _client.Dispose();
            _disposed = true;
        }
    }
}
