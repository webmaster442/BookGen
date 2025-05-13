//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Http;

public sealed class ConsoleHttpServerRunner : IDisposable
{
    private readonly CancellationTokenSource _tokenSource;
    private readonly IHttpServer _server;
    private bool _disposed;

    public ConsoleHttpServerRunner(IHttpServer server)
    {
        _tokenSource = new();
        _server = server;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _tokenSource.Dispose();
        _disposed = true;
    }

    public async Task RunServer()
    {
        await _server.StartAsync();
        Console.WriteLine("Press any key to stop the server...");
        Console.ReadKey();
        await _server.StopAsync();
    }
}
