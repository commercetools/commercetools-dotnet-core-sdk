using commercetools.Sdk.Domain.Carts;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class RemoveLineItemStagedAction : StagedOrderUpdateAction
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
