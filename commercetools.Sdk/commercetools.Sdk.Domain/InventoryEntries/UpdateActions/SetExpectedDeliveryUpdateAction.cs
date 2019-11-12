using System;

namespace commercetools.Sdk.Domain.InventoryEntries.UpdateActions
{
    public class SetExpectedDeliveryUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setExpectedDelivery";
        public DateTime? ExpectedDelivery { get; set; }
    }
}
