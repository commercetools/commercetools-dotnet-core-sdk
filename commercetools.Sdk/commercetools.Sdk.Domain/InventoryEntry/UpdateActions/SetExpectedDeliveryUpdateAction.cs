using System;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.InventoryEntries
{
    public class SetExpectedDeliveryUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setExpectedDelivery";
        public DateTime ExpectedDelivery { get; set; }
    }
}