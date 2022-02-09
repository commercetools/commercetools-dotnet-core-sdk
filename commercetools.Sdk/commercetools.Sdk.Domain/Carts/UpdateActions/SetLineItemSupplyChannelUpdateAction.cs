using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Channels;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class SetLineItemSupplyChannelUpdateAction : CartUpdateAction
    {
        public override string Action => "setLineItemSupplyChannel";
        [Required]
        public string LineItemId { get; set; }
        public ResourceIdentifier SupplyChannel { get; set; }
    }
}
