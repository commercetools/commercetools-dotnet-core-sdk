using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.Domain.Messages.Products
{
    [TypeMarker("ProductRemovedFromCategory")]
    public class ProductRemovedFromCategoryMessage : Message<Product>
    {
        public Reference<Category> Category { get; set; }
        public bool Staged { get; set; }
    }
}
