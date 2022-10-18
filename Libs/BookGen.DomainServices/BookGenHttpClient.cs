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

        public T? DownloadJson<T>(string url, JsonSerializerOptions? options = null) where T: class
        {
            string? str = Download(url);

            if (str == null)
                return null;

            return JsonSerializer.Deserialize<T>(str, options);
        }

        public string? Download(string url)
        {
            string encoded = HttpUtility.UrlEncode(url);

            using HttpResponseMessage? response = _client
                .GetAsync(encoded)
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                string? content = response
                    .Content
                    .ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                return content;
            }

            return null;
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
