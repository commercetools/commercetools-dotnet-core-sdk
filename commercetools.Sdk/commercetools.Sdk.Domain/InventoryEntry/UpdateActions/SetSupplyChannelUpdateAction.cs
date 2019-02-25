using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Channels;

namespace commercetools.Sdk.Domain.InventoryEntries
{
    public class SetSupplyChannelUpdateAction : UpdateAction<InventoryEntry>
    {
        public string Action => "setSupplyChannel";
        public Reference<Channel> SupplyChannel { get; set; }
    }
}