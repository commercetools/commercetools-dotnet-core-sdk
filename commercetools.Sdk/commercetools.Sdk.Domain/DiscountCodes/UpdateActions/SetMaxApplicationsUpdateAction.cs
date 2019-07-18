namespace commercetools.Sdk.Domain.DiscountCodes.UpdateActions
{
    public class SetMaxApplicationsUpdateAction : UpdateAction<DiscountCode>
    {
        public string Action => "setMaxApplications";
        public long MaxApplications { get; set; }
    }
}
