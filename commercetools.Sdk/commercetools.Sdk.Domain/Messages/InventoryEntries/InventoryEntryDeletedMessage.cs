using commercetools.Sdk.Domain.Channels;

namespace commercetools.Sdk.Domain.Messages.InventoryEntries
{
    [TypeMarker("InventoryEntryDeleted")]
    public class InventoryEntryDeletedMessage : Message<InventoryEntry>
    {
        public string Sku { get; set;}

        public Reference<Channel> SupplyChannel { get; set;}
    }
}
