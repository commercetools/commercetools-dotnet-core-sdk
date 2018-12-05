namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class SetMaxApplicationsPerCustomerUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setMaxApplicationsPerCustomer";
        public double MaxApplicationsPerCustomer { get; set; }
    }
}