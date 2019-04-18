using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Categories;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class RemoveFromCategoryUpdateAction : UpdateAction<Product>
    {
        public string Action => "removeFromCategory";
        [Required]
        public ResourceIdentifier<Category> Category { get; set; }
        public bool Staged { get; set; }
    }
}
