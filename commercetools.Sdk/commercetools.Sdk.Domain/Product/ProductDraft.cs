using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain
{
    public class ProductDraft : IDraft<Product>
    {
        public string Key { get; set; }
        [Required]
        public LocalizedString Name { get; set; }
        [Required]
        // TODO See if resource identifier should be generic and pointing to the type
        public ResourceIdentifier ProductType { get; set; }
        [Required]
        [Slug]
        public LocalizedString Slug { get; set; }
        public LocalizedString Description { get; set; }
        public List<ResourceIdentifier> Categories { get; set; }
        public CategoryOrderHints CategoryOrderHints { get; set; }
        public LocalizedString MetaTitle { get; set; }
        public LocalizedString MetaDescription { get; set; }
        public LocalizedString MetaKeywords { get; set; }
        // TODO See if master variant should be validated
        public ProductVariantDraft MasterVariant { get; set; }
        public List<ProductVariantDraft> Variants { get; set; }
        public Reference<TaxCategory> TaxCategory { get; set; }
        public Dictionary<string, List<SearchKeywords>> SearchKeywords { get; set; }
        public Reference<State> State { get; set; }
        public bool Publish { get; set; }
    }
}