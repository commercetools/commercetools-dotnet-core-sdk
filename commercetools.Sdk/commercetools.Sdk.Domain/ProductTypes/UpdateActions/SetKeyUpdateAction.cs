namespace commercetools.Sdk.Domain.ProductTypes.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<ProductType>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}