namespace commercetools.Sdk.Domain.Reviews
{
    public class SetCustomerUpdateAction : UpdateAction<Review>
    {
        public string Action => "setCustomer";
        public ResourceIdentifier Customer { get; set; }
    }
}