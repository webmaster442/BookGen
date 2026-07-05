//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Http;

public interface IHttpServer : IAsyncDisposable
{
    int Port { get; }
    IEnumerable<string> GetListenUrls();
    Task StartAsync();
    Task StopAsync();
}
