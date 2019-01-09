namespace commercetools.Sdk.Domain.Customers.UpdateActions
{
    public class SetSalutationUpdateAction : UpdateAction<Customer>
    {
        public string Action => "setSalutation";
        public string Salutation { get; set; }
    }
}
