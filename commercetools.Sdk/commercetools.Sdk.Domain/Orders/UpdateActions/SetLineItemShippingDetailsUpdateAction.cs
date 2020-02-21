using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetLineItemShippingDetailsUpdateAction : OrderUpdateAction
    {
        public override string Action => "setLineItemShippingDetails";
        [Required]
        public string LineItemId { get; set; }
        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}