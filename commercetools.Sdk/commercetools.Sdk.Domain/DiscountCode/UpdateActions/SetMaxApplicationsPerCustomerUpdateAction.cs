namespace commercetools.Sdk.Domain.DiscountCodes
{
    public class SetMaxApplicationsUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setMaxApplications";
        public double MaxApplications { get; set; }
    }
}