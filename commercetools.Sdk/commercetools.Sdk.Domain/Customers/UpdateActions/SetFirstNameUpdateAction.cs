namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetFirstNameUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setFirstName";
        public string FirstName { get; set; }
    }
}
