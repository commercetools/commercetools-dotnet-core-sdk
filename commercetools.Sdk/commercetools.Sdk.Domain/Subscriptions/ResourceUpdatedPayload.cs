using System;

namespace commercetools.Sdk.Domain.Subscriptions
{
    /// <summary>
    /// This payload will be sent for a ChangeSubscription if a resource was updated.
    /// </summary>
    [TypeMarker("ResourceUpdated")]
    public class ResourceUpdatedPayload<T> : Payload<T>
    {
        public long Version { get; set; }
        public long OldVersion { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}