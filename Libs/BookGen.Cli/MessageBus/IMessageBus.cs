namespace BookGen.Cli.MessageBus
{
    public interface IMessageBus
    {
        void Send<TMessage>(Guid target, TMessage message) where TMessage : MessageBase;
        void Broadcast<TMessage>(TMessage message) where TMessage : MessageBase;
        void RegisterCleint<TMessage>(IMessageClient<TMessage> client) where TMessage: MessageBase;
    }
}
