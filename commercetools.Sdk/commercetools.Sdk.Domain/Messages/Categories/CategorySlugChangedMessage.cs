using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.Domain.Messages.Categories
{
    [TypeMarker("CategorySlugChanged")]
    public class CategorySlugChangedMessage : Message<Category>
    {
        public LocalizedString Slug { get; set; }
    }
}
