using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class SetPricesUpdateAction : UpdateAction<Product>
    {
        public string Action => "setPrices";
        public int VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public List<PriceDraft> Price { get; set; }
        public bool Staged { get; set; }
    }
}