using commercetools.Sdk.Domain.Carts;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class ChangeLineItemQuantityStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "changeLineItemQuantity";
        [Required]
        public string LineItemId { get; set; }
        [Required]
        public long Quantity { get; set; }
        public BaseMoney ExternalPrice { get; set; }
        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }
    }
}
