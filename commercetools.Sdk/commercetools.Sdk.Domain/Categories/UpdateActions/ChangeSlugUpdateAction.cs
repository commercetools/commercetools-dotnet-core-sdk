using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Categories.UpdateActions
{
    public class ChangeSlugUpdateAction : UpdateAction<Category>
    {
        public string Action => "changeSlug";
        [Required]
        public LocalizedString Slug { get; set; }
    }
}