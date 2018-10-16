using System.Collections.Generic;

namespace commercetools.Sdk.Domain
{
    public class ProductData
    {
        public ProductVariant MasterVariant { get; set; }
        public LocalizedString Slug { get; set; }
        public LocalizedString Name { get; set; }
        public List<Reference> Categories { get; set; }
        public List<ProductVariant> Variants { get; set; }
        public Price Price { get; set; }
    }
}