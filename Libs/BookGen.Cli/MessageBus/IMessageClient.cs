//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.MessageBus
{
    public interface IMessageClient
    {
        Guid ClientId { get; }
    }

    public interface IMessageClient<in TMessage> : IMessageClient where TMessage : MessageBase
    {
        void HandleMessage(TMessage message);
    }
}
