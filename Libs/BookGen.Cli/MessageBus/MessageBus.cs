//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.MessageBus;

public sealed class MessageBus : IMessageBus
{
    private readonly Dictionary<Guid, MessageTarget> _clients;
    private readonly object _lock;

    public MessageBus()
    {
        _lock = new object();
        _clients = new Dictionary<Guid, MessageTarget>();
    }

    private void RemoveDead()
    {
        HashSet<Guid> deads = new HashSet<Guid>();
        foreach (var client in _clients)
        {
            if (!client.Value.IsAlive)
                deads.Add(client.Key);
        }
        foreach (var dead in deads)
        {
            _clients.Remove(dead);
        }
    }

    public void Broadcast<TMessage>(TMessage message) where TMessage : MessageBase
    {
        RemoveDead();
        foreach (var client in _clients.Values)
        {
            client.Invoke(message, _lock);
        }
    }

    public void RegisterCleint<TMessage>(IMessageClient<TMessage> client) where TMessage : MessageBase
    {
        RemoveDead();
        _clients.Add(client.ClientId, new MessageTarget(client));
    }

    public void Send<TMessage>(Guid target, TMessage message) where TMessage : MessageBase
    {
        RemoveDead();
        if (_clients.TryGetValue(target, out MessageTarget? messageTarget))
        {
            messageTarget.Invoke(message, _lock);
        }
    }
}
