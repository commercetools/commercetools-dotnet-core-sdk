using System;

namespace commercetools.Sdk.Domain
{
    public class Product
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public Reference<ProductType> ProductType { get; set; }
        public ProductCatalogData MasterData { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public Reference<State> State { get; set; }
        public ReviewRatingStatistics ReviewRatingStatistics { get; set; }
    }
}