using commercetools.Sdk.Domain.ProductProjections;

namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductCreated")]
    public class ProductCreatedMessage : Message<Product>
    {
        public ProductProjection ProductProjection { get; set; }
    }
}
