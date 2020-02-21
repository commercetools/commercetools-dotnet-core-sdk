using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetCustomLineItemShippingDetailsUpdateAction : OrderUpdateAction
    {
        public override string Action => "setCustomLineItemShippingDetails";
        [Required]
        public string CustomLineItemId { get; set; }
        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}