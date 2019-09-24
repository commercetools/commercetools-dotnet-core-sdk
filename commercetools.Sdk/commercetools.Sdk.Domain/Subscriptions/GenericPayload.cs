namespace commercetools.Sdk.Domain.Subscriptions
{
    public abstract class Payload<T> : Payload
    {
        public new Reference<T> Resource { get; set; }
    }
}
