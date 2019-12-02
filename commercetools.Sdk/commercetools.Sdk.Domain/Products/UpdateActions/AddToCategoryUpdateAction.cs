using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Categories;
using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class AddToCategoryUpdateAction : UpdateAction<Product>
    {
        public string Action => "addToCategory";
        [Required]
        public IReference<Category> Category { get; set; }
        public string OrderHint { get; set; }
        public bool Staged { get; set; }
    }
}