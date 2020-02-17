using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Carts.UpdateActions
{
    public class RemoveLineItemUpdateAction : CartUpdateAction
    {
        public override string Action => "removeLineItem";
        [Required]
        public string LineItemId { get; set; }
        public long Quantity { get; set; }
        public BaseMoney ExternalPrice { get; set; }
        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }
        public ItemShippingDetailsDraft ShippingDetailsToRemove { get; set; }
    }
}
