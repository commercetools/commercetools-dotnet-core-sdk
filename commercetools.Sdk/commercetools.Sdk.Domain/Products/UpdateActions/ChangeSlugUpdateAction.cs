using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Products
{
    public class ChangeSlugUpdateAction : UpdateAction<Product>
    {
        public string Action => "changeSlug";
        [Slug]
        public LocalizedString Slug { get; set; }
        public bool Staged { get; set; }

    }
}