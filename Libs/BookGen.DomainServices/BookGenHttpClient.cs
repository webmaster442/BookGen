//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Interfaces;
using System.Net;

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

        public async Task<HttpStatusCode> DownloadToFile(Uri url, FsPath output, ILog log)
        {
            using HttpResponseMessage? response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                using var outStream = output.CreateStream(log);
                await stream.CopyToAsync(outStream);
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
}
