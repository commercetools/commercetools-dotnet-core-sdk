namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetCustomerIdUpdateAction : OrderUpdateAction
    {
        public override string Action => "setCustomerId";
        public string CustomerId { get; set; }
    }
}