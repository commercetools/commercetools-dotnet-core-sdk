namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}