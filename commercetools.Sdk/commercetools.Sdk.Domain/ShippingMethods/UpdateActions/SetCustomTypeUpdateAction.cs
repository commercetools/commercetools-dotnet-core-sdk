namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class SetCustomTypeUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "setCustomType";
        public ResourceIdentifier Type { get; set; }
        public Fields Fields { get; set; }
    }
}