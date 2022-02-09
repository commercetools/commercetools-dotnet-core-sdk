using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.InventoryEntries;

namespace commercetools.Sdk.Domain.Messages.InventoryEntries
{
    [TypeMarker("InventoryEntryQuantitySet")]
    public class InventoryEntryQuantitySetMessage : Message<InventoryEntry>
    {
        public long OldQuantityOnStock { get; set; }
        public long NewQuantityOnStock { get; set; }
        public long OldAvailableQuantity { get; set; }
        public long NewAvailableQuantity { get; set; }
        public IReference<Channel> SupplyChannel { get; set; }
    }
}