namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductCreated")]
    public class ProductSlugChangedMessage : Message<Product>
    {
        public LocalizedString Slug { get; set; }
    }
}
