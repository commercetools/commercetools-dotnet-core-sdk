using System;
using commercetools.Sdk.Domain.Channels;

namespace commercetools.Sdk.Domain
{
    [Endpoint("inventory")]
    public class InventoryEntry
    {
        public string Id { get; set; }
        public string Sku { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public Reference<Channel> SupplyChannel { get; set; }
        public long QuantityOnStock { get; set; }
        public long AvailableQuantity { get; set; }
        public int RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public CustomFields Custom { get; set; }
    }
}
