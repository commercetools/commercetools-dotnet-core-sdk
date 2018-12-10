using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateItemShippingAddressUpdateAction : UpdateAction<Order>
    {
        public string Action => "updateItemShippingAddress";
        [Required]
        public Address Address { get; set; }
    }
}