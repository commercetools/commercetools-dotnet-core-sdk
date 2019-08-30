namespace commercetools.Sdk.Domain.Messages
{
    public class Message<T> : Message
    {
        public new Reference<T> Resource { get; set; }
    }
}
