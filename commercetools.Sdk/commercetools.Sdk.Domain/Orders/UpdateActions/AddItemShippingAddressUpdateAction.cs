namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class AddItemShippingAddressUpdateAction : OrderUpdateAction
    {
        public override string Action => "addItemShippingAddress";
        [Required]
        public Address Address { get; set; }
    }
}