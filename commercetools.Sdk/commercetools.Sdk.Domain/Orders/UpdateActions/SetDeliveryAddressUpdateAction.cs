using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetDeliveryAddressUpdateAction : OrderUpdateAction
    {
        public override string Action => "setDeliveryAddress";
        [Required]
        public string DeliveryId { get; set; }
        public Address Address { get; set; }
    }
}