using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class AddPriceUpdateAction : UpdateAction<Product>
    {
        public string Action => "addPrice";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public PriceDraft Price { get; set; }
        public bool Staged { get; set; }
    }
}