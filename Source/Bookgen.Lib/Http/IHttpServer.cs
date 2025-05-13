//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Http;

public interface IHttpServer
{
    int Port { get; }
    IEnumerable<string> GetListenUrls();
    Task StartAsync();
    Task StopAsync();
}