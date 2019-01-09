namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetMiddleNameUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setMiddleName";
        public string MiddleName { get; set; }
    }
}
