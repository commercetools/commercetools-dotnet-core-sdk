using System.Collections.Generic;
using commercetools.Sdk.Domain.ProductProjections;

namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductDeleted")]
    public class ProductDeletedMessage : Message<Product>
    {
        public List<string> RemovedImageUrls { get; set; }
        public ProductProjection CurrentProjection { get; set; }
    }
}
