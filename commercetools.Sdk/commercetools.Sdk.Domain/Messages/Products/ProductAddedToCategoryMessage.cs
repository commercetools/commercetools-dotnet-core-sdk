using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductAddedToCategory")]
    public class ProductAddedToCategoryMessage : Message<Product>
    {
        public Reference<Category> Category { get; set; }
        public bool Staged { get; set; }
    }
}
