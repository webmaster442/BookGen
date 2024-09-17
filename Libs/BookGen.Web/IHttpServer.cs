//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Web;

public interface IHttpServer
{
    int Port { get; }
    ICollection<string> GetListenUrls();
    Task StartAsync(CancellationToken cancellationToken);
}