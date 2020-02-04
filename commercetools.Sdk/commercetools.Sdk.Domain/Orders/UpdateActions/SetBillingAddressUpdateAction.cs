namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetBillingAddressUpdateAction : OrderUpdateAction
    {
        public override string Action => "setBillingAddress";
        public Address Address { get; set; }
    }
}