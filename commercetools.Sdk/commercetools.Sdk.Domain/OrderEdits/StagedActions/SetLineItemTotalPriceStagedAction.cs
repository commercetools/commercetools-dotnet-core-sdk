using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    public class SetLineItemTotalPriceStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setLineItemTotalPrice";
        [Required]
        public string LineItemId { get; set; }
        public ExternalLineItemTotalPrice ExternalTotalPrice { get; set; }
    }
}