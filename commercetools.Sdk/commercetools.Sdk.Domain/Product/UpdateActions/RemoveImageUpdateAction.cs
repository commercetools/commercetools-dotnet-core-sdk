using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class RemoveImageUpdateAction : UpdateAction<Product>
    {
        public string Action => "removeImage";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public bool Staged { get; set; }
    }
}