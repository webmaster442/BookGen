//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
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

    public async Task<TOutput> NotifyAsync<TInput, TOutput>(TInput message)
    {
        var call = _mediatables.OfType<IAsycNotifyable<TInput, TOutput>>().FirstOrDefault();
        if (call is not null)
        {
            return await call.OnNotifyAsync(message);
        }
        throw new InvalidOperationException($"No handler found for message of type {typeof(TInput).FullName} with return type {typeof(TOutput).FullName}");
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
