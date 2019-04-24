using commercetools.Sdk.Domain.Categories;
using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Reviews;
using commercetools.Sdk.Domain.States;

namespace commercetools.Sdk.Domain.ProductProjections
{
    [Endpoint("product-projections")]
    public class ProductProjection : Resource<ProductProjection>
    {
        public string Key { get; set; }
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
