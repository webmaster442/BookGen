namespace BookGen.Cli.MessageBus
{
    public class SimpleMessage<T> : MessageBase
    {
        public T Payload { get; }

        public SimpleMessage(Guid sender, T payload) : base(sender)
        {
            Payload = payload;
        }

        public override string ToString()
        {
            return $"DispatchTime: {DispatchTime}\r\nSender: {SenderId}\r\nPayload: {Payload}";
        }
    }
}
