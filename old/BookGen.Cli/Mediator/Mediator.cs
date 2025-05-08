//-----------------------------------------------------------------------------
// (c) 2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.Mediator;

public sealed class Mediator : IMediator, IDisposable
{
    private readonly List<IMediatable> _mediatables;

    public Mediator()
    {
        _mediatables = new List<IMediatable>();
    }

    public void Dispose()
    {
        _mediatables.Clear();
    }

    public void Notify<T>(T message)
    {
        foreach (var mediatable in _mediatables)
        {
            if (mediatable is INotifyable<T> client)
            {
                client.OnNotify(message);
            }
        }
    }

    public async Task NotifyAsync<T>(T message)
    {
        foreach (var mediatable in _mediatables)
        {
            if (mediatable is IAsycNotifyable<T> client)
            {
                await client.OnNotifyAsync(message);
            }
        }
    }

    public void Register(IMediatable mediatable)
    {
        _mediatables.Add(mediatable);
    }

    public void Unregister(IMediatable mediatable)
    {
        _mediatables.Remove(mediatable);
    }
}
