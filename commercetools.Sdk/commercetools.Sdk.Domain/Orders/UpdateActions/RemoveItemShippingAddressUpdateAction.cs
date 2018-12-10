using commercetools.Sdk.Domain.Carts;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveItemShippingAddressUpdateAction : UpdateAction<Order>
    {
        public string Action => "removeItemShippingAddress";
        [Required]
        public string AddressKey { get; set; }
    }
}