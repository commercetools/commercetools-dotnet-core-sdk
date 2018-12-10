using System;

namespace commercetools.Sdk.Domain.Orders
{
    public class SyncInfo
    {
        public Reference<Channel> Channel { get; set; }
        public string ExternalId { get; set; }
        public DateTime SyncedAt { get; set; }
    }
}