namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetCustomerNumberUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setCustomerNumber";
        public string CustomerNumber { get; set; }
    }
}
