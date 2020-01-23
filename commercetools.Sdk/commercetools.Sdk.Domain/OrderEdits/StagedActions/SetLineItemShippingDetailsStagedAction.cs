using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.OrderEdits.StagedActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemShippingDetailsStagedAction : StagedOrderUpdateAction
    {
        public override string Action => "setLineItemShippingDetails";
        [Required]
        public string LineItemId { get; set; }
        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}