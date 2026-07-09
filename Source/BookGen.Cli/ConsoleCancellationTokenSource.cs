//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

internal sealed class ConsoleCancellationTokenSource : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private bool _disposed;

    public ConsoleCancellationTokenSource()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += OnCancelKeyPress;
    }

    public CancellationToken Token
    {
        get
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return _cancellationTokenSource.Token;
        }
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        _cancellationTokenSource.Cancel();
        e.Cancel = true;
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        Console.CancelKeyPress -= OnCancelKeyPress;
        _disposed = true;
    }
}
