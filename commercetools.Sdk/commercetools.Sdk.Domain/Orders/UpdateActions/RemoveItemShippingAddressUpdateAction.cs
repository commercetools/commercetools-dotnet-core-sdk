using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class RemoveItemShippingAddressUpdateAction : OrderUpdateAction
    {
        public override string Action => "removeItemShippingAddress";
        [Required]
        public string AddressKey { get; set; }
    }
}