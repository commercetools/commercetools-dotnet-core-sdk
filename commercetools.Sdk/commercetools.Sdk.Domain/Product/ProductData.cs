using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ProductData
    {        
        public LocalizedString Name { get; set; }
        public List<Reference> Categories { get; set; }
        public CategoryOrderHints CategoryOrderHints { get; set; }
        public LocalizedString Description { get; set; }
        public LocalizedString Slug { get; set; }
        public LocalizedString MetaTitle { get; set; }
        public LocalizedString MetaDescription { get; set; }
        public LocalizedString MetaKeywords { get; set; }        
        public ProductVariant MasterVariant { get; set; }
        public List<ProductVariant> Variants { get; set; }
        public Dictionary<string, List<SearchKeywords>> SearchKeywords { get; set; }
    }
}