using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.InventoryEntries.UpdateActions
{
    public class SetSupplyChannelUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setSupplyChannel";
        public IReference<Channel> SupplyChannel { get; set; }
    }
}