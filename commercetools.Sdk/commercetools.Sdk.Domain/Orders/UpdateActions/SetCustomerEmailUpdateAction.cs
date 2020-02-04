namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetCustomerEmailUpdateAction : OrderUpdateAction
    {
        public override string Action => "setCustomerEmail";
        public string Email { get; set; }
    }
}