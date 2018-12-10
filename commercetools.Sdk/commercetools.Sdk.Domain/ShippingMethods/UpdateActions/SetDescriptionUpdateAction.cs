namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class SetDescriptionUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "setDescription";
        public string Description { get; set; }
    }
}