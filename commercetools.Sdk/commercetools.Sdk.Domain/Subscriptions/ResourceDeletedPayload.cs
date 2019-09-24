using System;

namespace commercetools.Sdk.Domain.Subscriptions
{
    /// <summary>
    /// This payload will be sent for a ChangeSubscription if a resource was deleted.
    /// </summary>
    [TypeMarker("ResourceDeleted")]
    public class ResourceDeletedPayload<T> : Payload<T>
    {
        public long Version { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}