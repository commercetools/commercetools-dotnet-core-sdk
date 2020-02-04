namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateItemShippingAddressUpdateAction : OrderUpdateAction
    {
        public override string Action => "updateItemShippingAddress";
        [Required]
        public Address Address { get; set; }
    }
}