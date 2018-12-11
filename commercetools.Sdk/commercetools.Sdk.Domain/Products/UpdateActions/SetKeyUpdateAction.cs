namespace commercetools.Sdk.Domain.Products
{
    public class SetKeyUpdateAction : UpdateAction<Product>
    {
        public string Action => "setKey";
        public string Key { get; set; }
    }
}