using commercetools.Sdk.Domain.Messages;

namespace commercetools.Sdk.Domain.Subscriptions
{
    [TypeMarker("Message")]
    public class MessageSubscriptionPayload<T> : Payload<T>
    {
        public PayloadNotIncluded PayloadNotIncluded { get; set; }
        public Message<T> Message { get; set; }
    }
}