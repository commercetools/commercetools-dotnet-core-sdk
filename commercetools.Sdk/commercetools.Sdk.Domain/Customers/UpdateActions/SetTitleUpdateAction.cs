namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetTitleUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setTitle";
        public string Title { get; set; }
    }
}
