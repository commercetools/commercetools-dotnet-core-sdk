using System.Collections.Generic;
using commercetools.Sdk.Domain.ProductProjections;
using commercetools.Sdk.Domain.Products.UpdateActions;

namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductPublished")]
    public class ProductPublishedMessage : Message<Product>
    {
        public PublishScope Scope { get; set; }
        public ProductProjection ProductProjection { get; set; }
        public List<string> RemovedImageUrls { get; set; }
    }
}
