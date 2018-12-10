namespace commercetools.Sdk.Domain.ShippingMethods.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<ShippingMethod>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}