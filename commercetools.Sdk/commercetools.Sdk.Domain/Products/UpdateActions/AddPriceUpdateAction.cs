using System.ComponentModel.DataAnnotations;

namespace commercetools.Sdk.Domain.Products.UpdateActions
{
    public class AddPriceUpdateAction : UpdateAction<Product>
    {
        public string Action => "addPrice";
        public int? VariantId { get; set; }
        public string Sku { get; set; }
        [Required]
        public PriceDraft Price { get; set; }
        public bool Staged { get; set; }

        private AddPriceUpdateAction(PriceDraft price, bool staged = true)
        {
            this.Price = price;
            this.Staged = staged;
        }
        public AddPriceUpdateAction(string sku, PriceDraft price, bool staged = true) : this(price,staged)
        {
            this.Sku = sku;
        }
        public AddPriceUpdateAction(int variantId, PriceDraft price, bool staged = true) : this(price,staged)
        {
            this.Sku = null;
            this.VariantId = variantId;
        }
    }
}
