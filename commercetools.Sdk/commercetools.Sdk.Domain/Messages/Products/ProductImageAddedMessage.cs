namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductImageAdded")]
    public class ProductImageAddedMessage : Message<Product>
    {
        public int VariantId { get; set; }
        public Image Image { get; set; }
        public bool Staged { get; set; }
    }
}
