namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class SetLocalizedDescriptionUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "setLocalizedDescription";
        public LocalizedString LocalizedDescription { get; set; }
    }
}