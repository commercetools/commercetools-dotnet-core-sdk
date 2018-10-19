using System;
using System.Collections.Generic;
using System.Text;

namespace commercetools.Sdk.Domain
{
    public class ProductProjection
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public Reference<ProductType> ProductType { get; set; }
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public LocalizedString Slug { get; set; }
        public List<Reference<Category>> Categories { get; set; }
        public CategoryOrderHints CategoryOrderHints { get; set; }
        public LocalizedString MetaTitle { get; set; }
        public LocalizedString MetaDescription { get; set; }
        public LocalizedString MetaKeywords { get; set; }
        public Dictionary<string, List<SearchKeywords>> SearchKeywords { get; set; }
        public bool HasStagedChanges { get; set; }
        public bool Published { get; set; }
        public ProductVariant MasterVariant { get; set; }
        public List<ProductVariant> Variants { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public Reference<State> State { get; set; }
        public ReviewRatingStatistics ReviewRatingStatistics { get; set; }
    }
}
