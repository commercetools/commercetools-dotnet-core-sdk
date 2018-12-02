using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.InventoryEntries
{
    public class SetSupplyChannelUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setSupplyChannel";
        public Reference<Channel> SupplyChannel { get; set; }
    }
}