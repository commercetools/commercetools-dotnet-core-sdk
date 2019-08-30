using System.Collections.Generic;

namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductVariantDeleted")]
    public class ProductVariantDeletedMessage : Message<Product>
    {
        public List<string> RemovedImageUrls { get; set; }
        public ProductVariant Variant { get; set; }
    }
}
