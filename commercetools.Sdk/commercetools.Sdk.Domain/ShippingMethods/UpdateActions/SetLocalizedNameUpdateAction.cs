namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class SetLocalizedNameUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "setLocalizedName";
        public LocalizedString LocalizedName { get; set; }
    }
}