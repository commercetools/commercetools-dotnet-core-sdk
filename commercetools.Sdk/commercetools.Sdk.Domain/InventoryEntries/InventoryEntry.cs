using System;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.InventoryEntries
{
    [Endpoint("inventory")]
    [ResourceType(ReferenceTypeId.InventoryEntry)]
    public class InventoryEntry : Resource<InventoryEntry>
    {
        public string Sku { get; set; }
        public Reference<Channel> SupplyChannel { get; set; }
        public long QuantityOnStock { get; set; }
        public long AvailableQuantity { get; set; }
        public int RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public CustomFields Custom { get; set; }
    }
}
