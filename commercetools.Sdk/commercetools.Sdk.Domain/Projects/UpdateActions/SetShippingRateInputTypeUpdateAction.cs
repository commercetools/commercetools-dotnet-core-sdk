namespace commercetools.Sdk.Domain.Projects.UpdateActions
{
    public class SetShippingRateInputTypeUpdateAction : UpdateAction<Project>
    {
        public string Action => "setShippingRateInputType";
        public ShippingRateInputType ShippingRateInputType { get; set; }
    }
}
