namespace BookGen.Cli.MessageBus
{
    internal class MessageTarget
    {
        private readonly WeakReference _clientReference;

        public MessageTarget(IMessageClient client)
        {
            _clientReference = new WeakReference(client);
        }

        public bool IsAlive => _clientReference.IsAlive;

        public void Invoke<TMessage>(TMessage message, object lockObject) where TMessage : MessageBase
        {
            if (_clientReference.Target is IMessageClient<TMessage> client)
            {
                lock (lockObject)
                {
                    client.HandleMessage(message);
                }
            }
        }
    }
}
