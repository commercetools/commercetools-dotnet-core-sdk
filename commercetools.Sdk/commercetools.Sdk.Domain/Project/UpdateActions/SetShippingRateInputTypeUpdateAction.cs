namespace commercetools.Sdk.Domain.Project.UpdateActions
{
    public class SetShippingRateInputTypeUpdateAction : UpdateAction<Project>
    {
        public string Action => "setShippingRateInputType";
        public ShippingRateInputType ShippingRateInputType { get; set; }
    }
}
