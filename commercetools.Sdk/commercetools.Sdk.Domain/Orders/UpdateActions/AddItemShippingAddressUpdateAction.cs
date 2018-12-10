using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class AddItemShippingAddressUpdateAction : UpdateAction<Order>
    {
        public string Action => "addItemShippingAddress";
        [Required]
        public Address Address { get; set; }
    }
}