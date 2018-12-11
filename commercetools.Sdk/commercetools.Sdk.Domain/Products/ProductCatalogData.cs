namespace commercetools.Sdk.Domain
{
    public class ProductCatalogData
    {
        public bool Published { get; set; }
        public ProductData Current { get; set; }
        public ProductData Staged { get; set; }
        public bool HasStagedChanges { get; set; }
    }
}