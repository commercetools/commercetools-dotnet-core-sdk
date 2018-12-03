using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class AddToCategoryUpdateAction : UpdateAction<Product>
    {
        public string Action => "addToCategory";
        [Required]
        public ResourceIdentifier Category { get; set; }
        public string OrderHint { get; set; }
        public bool Staged { get; set; }
    }
}