namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class SetKeyUpdateAction : UpdateAction<Product>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}