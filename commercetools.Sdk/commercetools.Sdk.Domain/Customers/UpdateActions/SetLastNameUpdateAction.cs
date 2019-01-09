namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetLastNameUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setLastName";
        public string LastName { get; set; }
    }
}
