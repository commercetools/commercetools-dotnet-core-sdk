namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetCompanyNameUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setCompanyName";
        public string CompanyName { get; set; }
    }
}
