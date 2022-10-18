using SkiaSharp;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Web;

namespace BookGen.DomainServices
{
    public sealed class BookGenHttpClient : IDisposable
    {
        private readonly HttpClient _client;
        private bool _disposed;

        public BookGenHttpClient()
        {
            _client = new HttpClient();
        }

        public HttpStatusCode TryDownload(string url, [NotNullWhen(true)] out string? result)
        {
            using HttpResponseMessage? response = _client
                .GetAsync(url)
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                string? content = response
                    .Content
                    .ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                result = content;
                return response.StatusCode;
            }

            result = null;
            return response.StatusCode;
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
}
