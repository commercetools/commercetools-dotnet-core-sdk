namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetShippingAddressUpdateAction : OrderUpdateAction
    {
        public override string Action => "setShippingAddress";
        public Address Address { get; set; }
    }
}