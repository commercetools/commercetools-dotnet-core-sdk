namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class SetMaxApplicationsPerCustomerUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setMaxApplicationsPerCustomer";
        public long MaxApplicationsPerCustomer { get; set; }
    }
}
