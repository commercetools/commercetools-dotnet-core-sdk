namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetCustomerGroupUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setCustomerGroup";
        public string CustomerGroup { get; set; }
    }
}
