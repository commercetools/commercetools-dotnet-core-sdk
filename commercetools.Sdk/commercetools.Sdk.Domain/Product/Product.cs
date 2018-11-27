namespace commercetools.Sdk.Domain
{
    public class Product
    {
        public string Id { get; set; }
        public string Key { get; set; }

        public ProductCatalogData MasterData { get; set; }
    }
}