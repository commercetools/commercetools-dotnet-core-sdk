namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetExternalIdUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setExternalId";
        public string ExternalId { get; set; }
    }
}
