//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Web;

public sealed class ConsoleCancellationSource : IDisposable
{
    private readonly CancellationTokenSource _tonenSource;
    private bool _disposed;

    public ConsoleCancellationSource()
    {
        _tonenSource = new();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _tonenSource.Dispose();
        _disposed = true;
    }

    public CancellationToken Token
        => _tonenSource.Token;

    public void Wait()
    {
        Console.WriteLine("Press any key to stop the server...");
        Console.ReadKey();
        _tonenSource.Cancel();
    }
}
