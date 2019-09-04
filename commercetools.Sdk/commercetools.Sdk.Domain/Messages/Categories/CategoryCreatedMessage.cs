using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.Domain.Messages.Categories
{
    [TypeMarker("CategoryCreated")]
    public class CategoryCreatedMessage : Message<Category>
    {
        public Category Category { get; set; }
    }
}
