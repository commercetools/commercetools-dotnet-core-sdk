namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetDefaultShippingAddressUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setDefaultShippingAddress";
        public string AddressId { get; set; }
    }
}
