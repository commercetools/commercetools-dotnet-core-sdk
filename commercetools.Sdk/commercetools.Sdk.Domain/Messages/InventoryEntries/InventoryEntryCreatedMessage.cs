using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.InventoryEntries;

namespace commercetools.Sdk.Domain.Messages.InventoryEntries
{
    [TypeMarker("InventoryEntryCreated")]
    public class InventoryEntryCreatedMessage : Message<InventoryEntry>
    {
        public InventoryEntry InventoryEntry { get; set; }
    }
}
