namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetDefaultBillingAddressUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setDefaultBillingAddress";
        public string AddressId { get; set; }
    }
}
