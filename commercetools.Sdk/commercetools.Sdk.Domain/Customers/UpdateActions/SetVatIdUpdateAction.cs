namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetVatIdUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setVatId";
        public string VatId { get; set; }
    }
}
