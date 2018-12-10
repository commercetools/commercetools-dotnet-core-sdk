using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class SetCustomLineItemShippingDetailsUpdateAction : UpdateAction<Order>
    {
        public string Action => "setCustomLineItemShippingDetails";
        [Required]
        public string CustomLineItemId { get; set; }
        public ItemShippingDetailsDraft ShippingDetails { get; set; }
    }
}