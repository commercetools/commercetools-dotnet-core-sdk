using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class RemoveFromCategoryUpdateAction : UpdateAction<Product>
    {
        public string Action => "removeFromCategory";
        [Required]
        public ResourceIdentifier Category { get; set; }
        public bool Staged { get; set; }
    }
}