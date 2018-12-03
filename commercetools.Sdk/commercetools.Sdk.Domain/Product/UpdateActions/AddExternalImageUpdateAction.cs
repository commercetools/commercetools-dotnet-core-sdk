using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class AddExternalImageUpdateAction : UpdateAction<Product>
    {
        public string Action => "addExternalImage";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public Image Image { get; set; }
        public bool Staged { get; set; }
    }
}