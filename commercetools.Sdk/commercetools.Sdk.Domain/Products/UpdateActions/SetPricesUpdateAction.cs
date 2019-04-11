using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products
{
    public class SetPricesUpdateAction : UpdateAction<Product>
    {
        public string Action => "setPrices";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public List<PriceDraft> Prices { get; set; }
        public bool Staged { get; set; }

        private SetPricesUpdateAction(List<PriceDraft> prices, bool staged = true)
        {
            this.Prices = prices;
            this.Staged = staged;
        }

        public SetPricesUpdateAction(string sku, List<PriceDraft> prices, bool staged = true) : this(prices, staged)
        {
            this.Sku = sku;
        }
        public SetPricesUpdateAction(int variantId, List<PriceDraft> prices, bool staged = true) : this(prices, staged)
        {
            this.VariantId = variantId;
        }
    }
}
