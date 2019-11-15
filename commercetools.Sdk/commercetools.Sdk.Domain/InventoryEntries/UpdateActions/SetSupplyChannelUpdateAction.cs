using commercetools.Sdk.Domain.Channels;

namespace commercetools.Sdk.Domain.InventoryEntries.UpdateActions
{
    public class SetSupplyChannelUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setSupplyChannel";
        public Reference<Channel> SupplyChannel { get; set; }
    }
}