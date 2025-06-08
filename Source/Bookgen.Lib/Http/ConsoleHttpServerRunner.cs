//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Http;

public sealed class ConsoleHttpServerRunner : IAsyncDisposable
{
    private readonly CancellationTokenSource _tokenSource;
    private bool _disposed;

    public IHttpServer Server { get; }

    public ConsoleHttpServerRunner(IHttpServer server)
    {
        _tokenSource = new();
        Server = server;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _tokenSource.Dispose();
        await Server.StopAsync();
        await Server.DisposeAsync();
        _disposed = true;
    }

    public async Task RunServer()
    {
        await Server.StartAsync();
        Console.WriteLine("Press any key to stop the server...");
        Console.ReadKey();
    }
}
