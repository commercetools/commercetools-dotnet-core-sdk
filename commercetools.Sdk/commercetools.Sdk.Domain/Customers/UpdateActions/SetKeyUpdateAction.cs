namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}
