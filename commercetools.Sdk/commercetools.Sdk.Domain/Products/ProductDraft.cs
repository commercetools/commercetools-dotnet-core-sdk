using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductDraft : IDraft<Product>
    {
        public string Key { get; set; }
        [Required]
        public LocalizedString Name { get; set; }
        [Required]
        public IReferenceable<ProductType> ProductType { get; set; }
        [Required]
        [Slug]
        public LocalizedString Slug { get; set; }
        public LocalizedString Description { get; set; }
        public List<IReferenceable<Category>> Categories { get; set; }
        public CategoryOrderHints CategoryOrderHints { get; set; }
        public LocalizedString MetaTitle { get; set; }
        public LocalizedString MetaDescription { get; set; }
        public LocalizedString MetaKeywords { get; set; }
        public ProductVariantDraft MasterVariant { get; set; }
        public List<ProductVariantDraft> Variants { get; set; }
        public IReferenceable<TaxCategory> TaxCategory { get; set; }
        public Dictionary<string, List<SearchKeywords>> SearchKeywords { get; set; }
        public IReferenceable<State> State { get; set; }
        public bool Publish { get; set; }
    }
}
