using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class SetImageLabelUpdateAction : UpdateAction<Product>
    {
        public string Action => "setImageLabel";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Label { get; set; }
        public bool Staged { get; set; }
    }
}