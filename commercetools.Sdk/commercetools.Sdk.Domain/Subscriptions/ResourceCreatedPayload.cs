using System;

namespace commercetools.Sdk.Domain.Subscriptions
{
    /// <summary>
    /// This payload will be sent for a ChangeSubscription if a resource was created.
    /// </summary>
    [TypeMarker("ResourceCreated")]
    public class ResourceCreatedPayload<T> : Payload<T>
    {
        public long Version { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}