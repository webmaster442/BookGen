//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json;

namespace BookGen.Vfs;

public interface IApiClient : IDisposable
{
    Task<T> DownloadJsonAsync<T>(Uri url, JsonSerializerOptions? options = null) where T : class;
    Task DownloadFileTo(Uri url, Stream target, IProgress<long> progress, CancellationToken cancellationToken);
}
